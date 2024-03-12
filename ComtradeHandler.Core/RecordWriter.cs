using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Comtrade.Core
{
    /// <summary>
    ///     For creating COMTRADE files
    ///     Currently, supported COMTRADE version 1999: ASCII or Binary, timestamp guided
    /// </summary>
    public class RecordWriter
    {
        private readonly List<AnalogChannelInformation> analogChannelInformationList;
        private readonly List<DigitalChannelInformation> digitalChannelInformationList;

        private readonly List<DataFileSample> sampleList;
        private readonly List<SampleRate> sampleRateList;

        /// <summary>
        ///     Time of first value in data
        /// </summary>
        public DateTime StartTime;

        /// <summary>
        ///     Time of trigger point
        /// </summary>
        public DateTime TriggerTime;

        /// <summary>
        ///     Create empty writer
        /// </summary>
        public RecordWriter()
        {
            sampleList = new List<DataFileSample>();
            analogChannelInformationList = new List<AnalogChannelInformation>();
            digitalChannelInformationList = new List<DigitalChannelInformation>();
        }

        /// <summary>
        ///     Create writer with data from reader
        /// </summary>
        public RecordWriter(RecordReader reader)
        {
            StationName = reader.Configuration.StationName;
            DeviceId = reader.Configuration.DeviceId;

            sampleList = new List<DataFileSample>(reader.Data.Samples);

            analogChannelInformationList =
                new List<AnalogChannelInformation>(reader.Configuration.AnalogChannelInformationList);

            digitalChannelInformationList =
                new List<DigitalChannelInformation>(reader.Configuration.DigitalChannelInformationList);

            sampleRateList = new List<SampleRate>(reader.Configuration.SampleRates);

            StartTime = reader.Configuration.StartTime;
            TriggerTime = reader.Configuration.TriggerTime;
        }

        /// <summary>
        /// </summary>
        public string StationName { get; }

        /// <summary>
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// </summary>
        public void AddAnalogChannel(AnalogChannelInformation analogChannel)
        {
            analogChannel.Index = analogChannelInformationList.Count + 1;
            analogChannelInformationList.Add(analogChannel);
        }

        /// <summary>
        /// </summary>
        public void AddDigitalChannel(DigitalChannelInformation digitalChannel)
        {
            digitalChannel.Index = digitalChannelInformationList.Count + 1;
            digitalChannelInformationList.Add(digitalChannel);
        }

        /// <summary>
        /// </summary>
        /// <param name="timestamp">micro second</param>
        /// <param name="analogs"></param>
        /// <param name="digitals"></param>
        public void AddSample(int timestamp, double[] analogs, bool[] digitals)
        {
            var notNullAnalogs = analogs;
            var notNullDigitals = digitals;
            if (analogs == null) notNullAnalogs = new double[0];
            if (digitals == null) notNullDigitals = new bool[0];

            if (analogChannelInformationList.Count != notNullAnalogs.Length) {
                throw new InvalidOperationException(
                    $"Analog count ({notNullAnalogs.Length}) must be equal to channels count ({analogChannelInformationList.Count})");
            }

            if (digitalChannelInformationList.Count != notNullDigitals.Length) {
                throw new InvalidOperationException(
                    $"Digital count ({notNullDigitals.Length}) must be equal to channels count ({digitalChannelInformationList.Count})");
            }

            sampleList.Add(new DataFileSample(sampleList.Count + 1, timestamp, notNullAnalogs, notNullDigitals));
        }

        /// <summary>
        ///     Support only Ascii or Binary file type
        /// </summary>
        public void SaveToFile(string fullPathToFile, DataFileType dataFileType = DataFileType.Binary)
        {
            if (dataFileType == DataFileType.Undefined ||
                dataFileType == DataFileType.Binary32 ||
                dataFileType == DataFileType.Float32) {
                throw new InvalidOperationException("Currently unsupported " + dataFileType);
            }


            var path = Path.GetDirectoryName(fullPathToFile);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPathToFile);

            CalculateScaleFactorAB(dataFileType);

            //CFG part
            var strings = new List<string>();

            strings.Add(StationName + GlobalSettings.Comma +
                        DeviceId + GlobalSettings.Comma +
                        "1999");

            strings.Add((analogChannelInformationList.Count + digitalChannelInformationList.Count).ToString() +
                        GlobalSettings.Comma +
                        analogChannelInformationList.Count + "A" + GlobalSettings.Comma +
                        digitalChannelInformationList.Count + "D");

            for (var i = 0; i < analogChannelInformationList.Count; i++) {
                strings.Add(analogChannelInformationList[i].ToCFGString());
            }

            for (var i = 0; i < digitalChannelInformationList.Count; i++) {
                strings.Add(digitalChannelInformationList[i].ToCFGString());
            }

            strings.Add("50.0");

            if (sampleRateList == null || sampleRateList.Count == 0) {
                strings.Add("0");

                strings.Add("0" + GlobalSettings.Comma +
                            sampleList.Count);
            }
            else {
                strings.Add(sampleRateList.Count.ToString());

                foreach (var sampleRate in sampleRateList) {
                    strings.Add(sampleRate.SamplingFrequency.ToString() + GlobalSettings.Comma +
                                sampleRate.LastSampleNumber);
                }
            }

            strings.Add(StartTime.ToString(GlobalSettings.DateTimeFormat,
                                           CultureInfo.InvariantCulture));

            strings.Add(TriggerTime.ToString(GlobalSettings.DateTimeFormat,
                                             CultureInfo.InvariantCulture));

            switch (dataFileType) {
                case DataFileType.ASCII:
                    strings.Add("ASCII");
                    break;
                case DataFileType.Binary:
                    strings.Add("BINARY");
                    break;
                case DataFileType.Binary32:
                    strings.Add("BINARY32");
                    break;
                case DataFileType.Float32:
                    strings.Add("FLOAT32");
                    break;
                default:
                    throw new InvalidOperationException("Undefined data file type =" + dataFileType);
            }

            strings.Add("1.0");

            File.WriteAllLines(Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionCFG, strings);

            //DAT part
            var dataFileFullPath = Path.Combine(path, fileNameWithoutExtension) + GlobalSettings.ExtensionDAT;

            if (dataFileType == DataFileType.ASCII) {
                strings = new List<string>();

                foreach (var sample in sampleList) {
                    strings.Add(sample.ToASCIIDAT());
                }

                File.WriteAllLines(dataFileFullPath, strings);
            }
            else {
                var bytes = new List<byte>();

                foreach (var sample in sampleList) {
                    bytes.AddRange(sample.ToByteDAT(dataFileType, analogChannelInformationList));
                }

                File.WriteAllBytes(dataFileFullPath, bytes.ToArray());
            }
        }

        private void CalculateScaleFactorAB(DataFileType dataFileType)
        {
            if (dataFileType == DataFileType.Binary ||
                dataFileType == DataFileType.Binary32)
                //i make it same, but in theory, bin32 can be more precise
            {
                for (var i = 0; i < analogChannelInformationList.Count; i++) {
                    var min = sampleList.Min(x => x.AnalogValues[i]);
                    var max = sampleList.Max(x => x.AnalogValues[i]);
                    analogChannelInformationList[i].MultiplierB = (max + min) / 2.0;
                    if (max != min) analogChannelInformationList[i].MultiplierA = (max - min) / 32767.0; //65536
                    analogChannelInformationList[i].Min = -32767; //by standart 1999
                    analogChannelInformationList[i].Max = 32767; //by standart 1999
                }
            }
            else if (dataFileType == DataFileType.ASCII) {
                foreach (var analogChannelInformation in analogChannelInformationList) {
                    analogChannelInformation.Min = -32767; //by standart 1999
                    analogChannelInformation.Max = 32767; //by standart 1999
                }
            }
        }
    }
}
