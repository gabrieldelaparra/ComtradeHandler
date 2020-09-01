using System;
using System.Collections.Generic;

namespace ComtradeHandler.Core
{
    /// <summary>
    ///
    /// </summary>
    public class DataFileSample
    {
        public int Number { get; }

        /// <summary>
        /// microsecond or nanosecond defined by CFG (according STD)
        /// Note: I do not know, where in CFG it is defined, suppose always be microsecond
        /// </summary>
        public int Timestamp { get; }

        public double[] AnalogValues { get; }
        public bool[] DigitalValues { get; }

        public DataFileSample(int number, int timestamp, double[] analogValues, bool[] digitalValues)
        {
            this.Number = number;
            this.Timestamp = timestamp;
            this.AnalogValues = analogValues;
            this.DigitalValues = digitalValues;
        }

        public DataFileSample(string asciiLine, int analogCount, int digitalCount)
        {
            asciiLine = asciiLine.Replace(GlobalSettings.WhiteSpace.ToString(), string.Empty);
            var strings = asciiLine.Split(GlobalSettings.Comma);

            this.AnalogValues = new double[analogCount];
            this.DigitalValues = new bool[digitalCount];

            this.Number = Convert.ToInt32(strings[0]);
            this.Timestamp = Convert.ToInt32(strings[1]);

            for (int i = 0; i < analogCount; i++)
            {
                if (strings[i + 2] != string.Empty)
                {//by Standard, can be missing value. In that case by default=0
                    this.AnalogValues[i] = Convert.ToDouble(strings[i + 2], System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            for (int i = 0; i < digitalCount; i++)
            {
                this.DigitalValues[i] = Convert.ToBoolean(Convert.ToInt32(strings[i + 2 + analogCount]));
            }
        }

        public DataFileSample(byte[] bytes, DataFileType dataFileType, int analogCount, int digitalCount)
        {
            this.AnalogValues = new double[analogCount];
            this.DigitalValues = new bool[digitalCount];

            this.Number = System.BitConverter.ToInt32(bytes, 0);
            this.Timestamp = System.BitConverter.ToInt32(bytes, 4);

            int digitalByteStart;
            if (dataFileType == DataFileType.Binary)
            {
                for (int i = 0; i < analogCount; i++)
                {
                    this.AnalogValues[i] = System.BitConverter.ToInt16(bytes, 8 + i * 2);
                }
                digitalByteStart = 8 + 2 * analogCount;
            }
            else
            {
                if (dataFileType == DataFileType.Binary32)
                {
                    for (int i = 0; i < analogCount; i++)
                    {//TODO add test
                        this.AnalogValues[i] = System.BitConverter.ToInt32(bytes, 8 + i * 4);
                    }
                }
                else if (dataFileType == DataFileType.Float32)
                {
                    for (int i = 0; i < analogCount; i++)
                    {//TODO add test
                        this.AnalogValues[i] = System.BitConverter.ToSingle(bytes, 8 + i * 4);
                    }
                }

                digitalByteStart = 8 + 4 * analogCount;
            }

            int digitalByteCount = DataFileHandler.GetDigitalByteCount(digitalCount);
            for (int i = 0; i < digitalCount; i++)
            {
                int digitalByteIterator = i / 8;
                this.DigitalValues[i] = Convert.ToBoolean((bytes[digitalByteStart + digitalByteIterator] >> (i - digitalByteIterator * 8)) & 1);
            }
        }

        public string ToASCIIDAT()
        {
            string result = string.Empty;
            result += this.Number.ToString();
            result += GlobalSettings.Comma +
                this.Timestamp.ToString();
            foreach (var analog in this.AnalogValues)
            {
                result += GlobalSettings.Comma +
                    analog.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            foreach (var digital in this.DigitalValues)
            {
                result += GlobalSettings.Comma +
                    System.Convert.ToInt32(digital).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            return result;
        }


        public byte[] ToByteDAT(DataFileType dataFileType, IReadOnlyList<AnalogChannelInformation> analogInformations)
        {
            var result = new byte[DataFileHandler.GetByteCount(this.AnalogValues.Length, this.DigitalValues.Length, dataFileType)];
            int analogOneChannelLength = dataFileType == DataFileType.Binary ? 2 : 4;
            int digitalByteStart = 8 + analogOneChannelLength * this.AnalogValues.Length;

            System.BitConverter.GetBytes(this.Number).CopyTo(result, 0);
            System.BitConverter.GetBytes(this.Timestamp).CopyTo(result, 4);

            switch (dataFileType)
            {
                case DataFileType.Binary:
                    this.AnalogsToBinaryDAT(result, analogInformations);
                    break;
                case DataFileType.Binary32:
                    this.AnalogsToBinary32DAT(result, analogInformations);
                    break;
                case DataFileType.Float32:
                    this.AnalogsToFloat32DAT(result);
                    break;
                default:
                    throw new InvalidOperationException("Not supported file type DAT");
            }

            this.DigitalsToDAT(result, digitalByteStart);
            return result;
        }

        void AnalogsToBinaryDAT(byte[] result, IReadOnlyList<AnalogChannelInformation> analogInformations)
        {
            for (int i = 0; i < this.AnalogValues.Length; i++)
            {
                short s = (short)((this.AnalogValues[i] - analogInformations[i].MultiplierB) / analogInformations[i].MultiplierA);
                System.BitConverter.GetBytes(s).CopyTo(result, 8 + i * 2);
            }
        }

        void AnalogsToBinary32DAT(byte[] result, IReadOnlyList<AnalogChannelInformation> analogInformations)
        {
            for (int i = 0; i < this.AnalogValues.Length; i++)
            {
                int s = (int)((this.AnalogValues[i] - analogInformations[i].MultiplierB) / analogInformations[i].MultiplierA);
                System.BitConverter.GetBytes(s).CopyTo(result, 8 + i * 4);
            }
        }

        void AnalogsToFloat32DAT(byte[] result)
        {
            for (int i = 0; i < this.AnalogValues.Length; i++)
            {
                System.BitConverter.GetBytes((float)this.AnalogValues[i]).CopyTo(result, 8 + i * 4);
            }
        }

        void DigitalsToDAT(byte[] result, int digitalByteStart)
        {
            int byteIndex = 0;
            byte s = 0;
            for (int i = 0; i < this.DigitalValues.Length; i++)
            {
                s = (byte)(System.Convert.ToInt32(s) | (System.Convert.ToInt32(this.DigitalValues[i]) << (i - byteIndex * 8)));

                if ((i + 1) % 8 == 0 || (i + 1) == this.DigitalValues.Length)
                {
                    result[digitalByteStart + byteIndex] = s;
                    s = 0;
                    byteIndex++;
                }
            }
        }
    }
}
