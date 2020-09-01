using System;
using System.Collections.Generic;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Working with *.cfg
    /// </summary>
    public class ConfigurationHandler
    {
        //first line

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string DeviceId { get; set; }

        public ComtradeVersion Version { get; set; } = ComtradeVersion.V1991;

        //second line
        public int AnalogChannelsCount { get; set; }
        public int DigitalChannelsCount { get; set; }
        private List<AnalogChannelInformation> _analogChannelInformationList;
        /// <summary>
        /// List of analog channel information
        /// </summary>
        public IReadOnlyList<AnalogChannelInformation> AnalogChannelInformationList => this._analogChannelInformationList;

        List<DigitalChannelInformation> _digitalChannelInformationList;
        /// <summary>
        /// List of digital channel information
        /// </summary>
        public IReadOnlyList<DigitalChannelInformation> DigitalChannelInformationList => this._digitalChannelInformationList;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public double Frequency { get; set; } = 50.0;

        public int SamplingRateCount { get; set; } 
        public List<SampleRate> SampleRates;

        /// <summary>
        /// Time of first value in data
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Time of trigger point
        /// </summary>
        public DateTime TriggerTime { get; private set; }

        public DataFileType DataFileType = DataFileType.Undefined;

        internal double TimeMultiplicationFactor = 1.0;

        public ConfigurationHandler()//для тестов
        {

        }

        public ConfigurationHandler(string fullPathToFileCFG)
        {
            this.Parse(System.IO.File.ReadAllLines(fullPathToFileCFG, System.Text.Encoding.Default));
        }

        public void Parse(string[] strings)
        {
            this.ParseFirstLine(strings[0]);
            this.ParseSecondLine(strings[1]);

            this._analogChannelInformationList = new List<AnalogChannelInformation>();
            for (int i = 0; i < this.AnalogChannelsCount; i++)
            {
                this._analogChannelInformationList.Add(new AnalogChannelInformation(strings[2 + i]));
            }

            this._digitalChannelInformationList = new List<DigitalChannelInformation>();
            for (int i = 0; i < this.DigitalChannelsCount; i++)
            {
                this._digitalChannelInformationList.Add(new DigitalChannelInformation(strings[2 + i + this.AnalogChannelsCount]));
            }

            var strIndex = 2 + this.AnalogChannelsCount + this.DigitalChannelsCount;
            this.ParseFrequencyLine(strings[strIndex++]);
            //strIndex++;

            this.ParseNumberOfSampleRates(strings[strIndex++]);
            //strIndex++;

            this.SampleRates = new List<SampleRate>();
            if (this.SamplingRateCount == 0)
            {
                this.SampleRates.Add(new SampleRate(strings[strIndex++]));
                //strIndex++;
            }
            else
            {
                for (var i = 0; i < this.SamplingRateCount; i++)
                {
                    this.SampleRates.Add(new SampleRate(strings[strIndex + i]));
                }
                strIndex += this.SamplingRateCount;
            }

            this.StartTime = ParseDateTime(strings[strIndex++]);
            this.TriggerTime = ParseDateTime(strings[strIndex++]);

            this.ParseDataFileType(strings[strIndex++]);

            this.ParseTimeMultiplicationFactor(strings[strIndex++]);

            //TODO там остаток ещё пропущен (но он только для стандарта 2013 года)
        }

        void ParseFirstLine(string firstLine)
        {
            firstLine = firstLine.Replace(GlobalSettings.WhiteSpace.ToString(), string.Empty);
            var values = firstLine.Split(GlobalSettings.Comma);
            this.StationName = values[0];
            this.DeviceId = values[1];
            if (values.Length == 3)
            {
                this.Version = ComtradeVersionConverter.Get(values[2]);
            }
        }

        void ParseSecondLine(string secondLine)
        {
            secondLine = secondLine.Replace(GlobalSettings.WhiteSpace.ToString(), string.Empty);
            var values = secondLine.Split(GlobalSettings.Comma);
            //values[0];//не используется, равен сумме двух последующих
            this.AnalogChannelsCount = Convert.ToInt32(values[1].TrimEnd('A'), System.Globalization.CultureInfo.InvariantCulture);
            this.DigitalChannelsCount = Convert.ToInt32(values[2].TrimEnd('D'), System.Globalization.CultureInfo.InvariantCulture);
        }

        void ParseFrequencyLine(string frequenceLine)
        {
            this.Frequency = Convert.ToDouble(frequenceLine.Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
        }

        void ParseNumberOfSampleRates(string str)
        {
            this.SamplingRateCount = Convert.ToInt32(str.Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
        }

        internal static DateTime ParseDateTime(string str)
        {   // "dd/mm/yyyy,hh:mm:ss.ssssss"
            DateTime result;
            DateTime.TryParseExact(str, GlobalSettings.DateTimeFormat,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   System.Globalization.DateTimeStyles.AllowWhiteSpaces,
                                   out result);
            return result;
        }

        void ParseDataFileType(string str)
        {
            this.DataFileType = DataFileTypeConverter.Get(str.Trim(GlobalSettings.WhiteSpace));
        }

        void ParseTimeMultiplicationFactor(string str)
        {
            this.TimeMultiplicationFactor = Convert.ToDouble(str.Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}