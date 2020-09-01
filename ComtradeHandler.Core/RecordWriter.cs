using System;
using System.Collections.Generic;
using System.Linq;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// For creating COMTRADE files
    /// Currently, supported COMTRADE version 1999: ASCII or Binary, timestamp guided
    /// </summary>
    public class RecordWriter
    {
        /// <summary>
        ///
        /// </summary>
        public string StationName { get; }

        /// <summary>
        ///
        /// </summary>
        public string DeviceId { get; }

        List<DataFileSample> sampleList;
        List<AnalogChannelInformation> analogChannelInformationList;
        List<DigitalChannelInformation> digitalChannelInformationList;
        List<SampleRate> sampleRateList;

        /// <summary>
        /// Time of first value in data
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        /// Time of trigger point
        /// </summary>
        public DateTime TriggerTime;

        /// <summary>
        /// Create empty writer
        /// </summary>
        public RecordWriter()
        {
            this.sampleList = new List<DataFileSample>();
            this.analogChannelInformationList = new List<AnalogChannelInformation>();
            this.digitalChannelInformationList = new List<DigitalChannelInformation>();
        }

        /// <summary>
        /// Create writer with data from reader
        /// </summary>
        public RecordWriter(RecordReader reader)
        {
            this.StationName = reader.Configuration.StationName;
            this.DeviceId = reader.Configuration.DeviceId;

            this.sampleList = new List<DataFileSample>(reader.Data.Samples);
            this.analogChannelInformationList = new List<AnalogChannelInformation>(reader.Configuration.AnalogChannelInformationList);
            this.digitalChannelInformationList = new List<DigitalChannelInformation>(reader.Configuration.DigitalChannelInformationList);
            this.sampleRateList = new List<SampleRate>(reader.Configuration.SampleRates);

            this.StartTime = reader.Configuration.StartTime;
            this.TriggerTime = reader.Configuration.TriggerTime;
        }

        /// <summary>
        ///
        /// </summary>
        public void AddAnalogChannel(AnalogChannelInformation analogChannel)
        {
            analogChannel.Index = this.analogChannelInformationList.Count + 1;
            this.analogChannelInformationList.Add(analogChannel);
        }

        /// <summary>
        ///
        /// </summary>
        public void AddDigitalChannel(DigitalChannelInformation digitalChannel)
        {
            digitalChannel.Index = this.digitalChannelInformationList.Count + 1;
            this.digitalChannelInformationList.Add(digitalChannel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timestamp">micro second</param>
        /// <param name="analogs"></param>
        /// <param name="digitals"></param>
        public void AddSample(int timestamp, double[] analogs, bool[] digitals)
        {
            double[] notNullAnalogs = analogs;
            bool[] notNullDigitals = digitals;
            if (analogs == null)
            {
                notNullAnalogs = new double[0];
            }
            if (digitals == null)
            {
                notNullDigitals = new bool[0];
            }

            if (this.analogChannelInformationList.Count != notNullAnalogs.Length)
            {
                throw new InvalidOperationException($"Analog count ({notNullAnalogs.Length}) must be equal to channels count ({analogChannelInformationList.Count})");
            }

            if (this.digitalChannelInformationList.Count != notNullDigitals.Length)
            {
                throw new InvalidOperationException($"Digital count ({notNullDigitals.Length}) must be equal to channels count ({digitalChannelInformationList.Count})");
            }

            this.sampleList.Add(new DataFileSample(this.sampleList.Count + 1, timestamp, notNullAnalogs, notNullDigitals));
        }

        /// <summary>
        /// Support only Ascii or Binary file type
        /// </summary>
        public void SaveToFile(string fullPathToFile, DataFileType dataFileType = DataFileType.Binary)
        {
            if (dataFileType == DataFileType.Undefined ||
               dataFileType == DataFileType.Binary32 ||
               dataFileType == DataFileType.Float32)
            {
                throw new InvalidOperationException("Currently unsupported " + dataFileType.ToString());
            }


            string path = System.IO.Path.GetDirectoryName(fullPathToFile);
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fullPathToFile);

            this.CalculateScaleFactorAB(dataFileType);

            //CFG part
            var strings = new List<string>();

            strings.Add(this.StationName + GlobalSettings.Comma +
                        this.DeviceId + GlobalSettings.Comma +
                        "1999");

            strings.Add((this.analogChannelInformationList.Count + this.digitalChannelInformationList.Count).ToString() + GlobalSettings.Comma +
                        this.analogChannelInformationList.Count.ToString() + "A" + GlobalSettings.Comma +
                        this.digitalChannelInformationList.Count.ToString() + "D");

            for (int i = 0; i < this.analogChannelInformationList.Count; i++)
            {
                strings.Add(this.analogChannelInformationList[i].ToCFGString());
            }

            for (int i = 0; i < this.digitalChannelInformationList.Count; i++)
            {
                strings.Add(this.digitalChannelInformationList[i].ToCFGString());
            }

            strings.Add("50.0");

            if (this.sampleRateList == null || this.sampleRateList.Count == 0)
            {
                strings.Add("0");
                strings.Add("0" + GlobalSettings.Comma +
                            this.sampleList.Count.ToString());
            }
            else
            {
                strings.Add(this.sampleRateList.Count.ToString());
                foreach (var sampleRate in this.sampleRateList)
                {
                    strings.Add(sampleRate.SamplingFrequency.ToString() + GlobalSettings.Comma +
                                sampleRate.LastSampleNumber.ToString());
                }
            }

            strings.Add(this.StartTime.ToString(GlobalSettings.DateTimeFormat,
                                   System.Globalization.CultureInfo.InvariantCulture));

            strings.Add(this.TriggerTime.ToString(GlobalSettings.DateTimeFormat,
                                   System.Globalization.CultureInfo.InvariantCulture));

            switch (dataFileType)
            {
                case DataFileType.ASCII: strings.Add("ASCII"); break;
                case DataFileType.Binary: strings.Add("BINARY"); break;
                case DataFileType.Binary32: strings.Add("BINARY32"); break;
                case DataFileType.Float32: strings.Add("FLOAT32"); break;
                default:
                    throw new InvalidOperationException("Undefined data file type =" + dataFileType.ToString());
            }

            strings.Add("1.0");

            System.IO.File.WriteAllLines(System.IO.Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionCFG, strings);

            //DAT part
            string dataFileFullPath = System.IO.Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionDAT;

            if (dataFileType == DataFileType.ASCII)
            {
                strings = new List<string>();
                foreach (var sample in this.sampleList)
                {
                    strings.Add(sample.ToASCIIDAT());
                }
                System.IO.File.WriteAllLines(dataFileFullPath, strings);
            }
            else
            {
                var bytes = new List<byte>();
                foreach (var sample in this.sampleList)
                {
                    bytes.AddRange(sample.ToByteDAT(dataFileType, this.analogChannelInformationList));
                }
                System.IO.File.WriteAllBytes(dataFileFullPath, bytes.ToArray());
            }
        }

        void CalculateScaleFactorAB(DataFileType dataFileType)
        {
            if (dataFileType == DataFileType.Binary ||
               dataFileType == DataFileType.Binary32)
            {//i make it same, but in theory, bin32 can be more precise
                for (int i = 0; i < this.analogChannelInformationList.Count; i++)
                {
                    double min = this.sampleList.Min(x => x.AnalogValues[i]);
                    double max = this.sampleList.Max(x => x.AnalogValues[i]);
                    this.analogChannelInformationList[i].MultiplierB = (max + min) / 2.0;
                    if (max != min)
                    {
                        this.analogChannelInformationList[i].MultiplierA = (max - min) / 32767.0;//65536
                    }
                    this.analogChannelInformationList[i].Min = -32767;//by standart 1999
                    this.analogChannelInformationList[i].Max = 32767;//by standart 1999
                }
            }
            else if (dataFileType == DataFileType.ASCII)
            {
                foreach (var analogChannelInformation in this.analogChannelInformationList)
                {
                    analogChannelInformation.Min = -32767;//by standart 1999
                    analogChannelInformation.Max = 32767;//by standart 1999
                }
            }
        }

    }
}
