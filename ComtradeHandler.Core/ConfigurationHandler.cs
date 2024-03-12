using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Comtrade.Core
{
    /// <summary>
    ///     Working with *.cfg
    /// </summary>
    public class ConfigurationHandler
    {
        private List<AnalogChannelInformation> _analogChannelInformationList;

        private List<DigitalChannelInformation> _digitalChannelInformationList;

        public DataFileType DataFileType = DataFileType.Undefined;
        public List<SampleRate> SampleRates;

        internal double TimeMultiplicationFactor = 1.0;

        //For testing
        public ConfigurationHandler()
        {
        }

        public ConfigurationHandler(string fullPathToFileCFG)
        {
            Parse(File.ReadAllLines(fullPathToFileCFG, Encoding.Default));
        }
        //first line

        /// <summary>
        ///     According STD for COMTRADE
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        ///     According STD for COMTRADE
        /// </summary>
        public string DeviceId { get; set; }

        public ComtradeVersion Version { get; set; } = ComtradeVersion.V1991;

        //second line
        public int AnalogChannelsCount { get; set; }
        public int DigitalChannelsCount { get; set; }

        /// <summary>
        ///     List of analog channel information
        /// </summary>
        public IReadOnlyList<AnalogChannelInformation> AnalogChannelInformationList => _analogChannelInformationList;

        /// <summary>
        ///     List of digital channel information
        /// </summary>
        public IReadOnlyList<DigitalChannelInformation> DigitalChannelInformationList => _digitalChannelInformationList;

        /// <summary>
        ///     According STD for COMTRADE
        /// </summary>
        public double Frequency { get; set; } = 50.0;

        public int SamplingRateCount { get; set; }

        /// <summary>
        ///     Time of first value in data
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        ///     Time of trigger point
        /// </summary>
        public DateTime TriggerTime { get; private set; }

        public void Parse(string[] strings)
        {
            ParseFirstLine(strings[0]);
            ParseSecondLine(strings[1]);

            _analogChannelInformationList = new List<AnalogChannelInformation>();

            for (var i = 0; i < AnalogChannelsCount; i++) {
                _analogChannelInformationList.Add(new AnalogChannelInformation(strings[2 + i]));
            }

            _digitalChannelInformationList = new List<DigitalChannelInformation>();

            for (var i = 0; i < DigitalChannelsCount; i++) {
                _digitalChannelInformationList.Add(new DigitalChannelInformation(strings[2 + i + AnalogChannelsCount]));
            }

            var strIndex = 2 + AnalogChannelsCount + DigitalChannelsCount;
            ParseFrequencyLine(strings[strIndex++]);
            //strIndex++;

            ParseNumberOfSampleRates(strings[strIndex++]);
            //strIndex++;

            SampleRates = new List<SampleRate>();

            if (SamplingRateCount == 0) {
                SampleRates.Add(new SampleRate(strings[strIndex++]));
                //strIndex++;
            }
            else {
                for (var i = 0; i < SamplingRateCount; i++) {
                    SampleRates.Add(new SampleRate(strings[strIndex + i]));
                }

                strIndex += SamplingRateCount;
            }

            StartTime = ParseDateTime(strings[strIndex++]);
            TriggerTime = ParseDateTime(strings[strIndex++]);

            ParseDataFileType(strings[strIndex++]);

            ParseTimeMultiplicationFactor(strings[strIndex++]);

            //TODO там остаток ещё пропущен (но он только для стандарта 2013 года)
        }

        private void ParseFirstLine(string firstLine)
        {
            firstLine = firstLine.Replace(GlobalSettings.WhiteSpace.ToString(), string.Empty);
            var values = firstLine.Split(GlobalSettings.Comma);
            StationName = values[0];
            DeviceId = values[1];
            if (values.Length == 3) Version = ComtradeVersionConverter.Get(values[2]);
        }

        private void ParseSecondLine(string secondLine)
        {
            secondLine = secondLine.Replace(GlobalSettings.WhiteSpace.ToString(), string.Empty);
            var values = secondLine.Split(GlobalSettings.Comma);
            //values[0];// not used, equal to the sum of the next two
            AnalogChannelsCount = Convert.ToInt32(values[1].TrimEnd('A'), CultureInfo.InvariantCulture);
            DigitalChannelsCount = Convert.ToInt32(values[2].TrimEnd('D'), CultureInfo.InvariantCulture);
        }

        private void ParseFrequencyLine(string frequenceLine)
        {
            Frequency = Convert.ToDouble(frequenceLine.Trim(), CultureInfo.InvariantCulture);
        }

        private void ParseNumberOfSampleRates(string str)
        {
            SamplingRateCount = Convert.ToInt32(str.Trim(), CultureInfo.InvariantCulture);
        }

        internal static DateTime ParseDateTime(string str)
        {
            // "dd/mm/yyyy,hh:mm:ss.ssssss"
            DateTime.TryParseExact(str, GlobalSettings.DateTimeFormat,
                                   CultureInfo.InvariantCulture,
                                   DateTimeStyles.AllowWhiteSpaces,
                                   out var result);

            return result;
        }

        private void ParseDataFileType(string str)
        {
            DataFileType = DataFileTypeConverter.Get(str.Trim());
        }

        private void ParseTimeMultiplicationFactor(string str)
        {
            TimeMultiplicationFactor = Convert.ToDouble(str.Trim(), CultureInfo.InvariantCulture);
        }
    }
}
