using System;
using System.Collections.Generic;
using System.IO;

namespace Comtrade.Core
{
    /// <summary>
    ///     Class for parsing comtrade files
    /// </summary>
    public class RecordReader
    {
        internal RecordReader()
        {
        }

        /// <summary>
        ///     Read record from file
        /// </summary>
        public RecordReader(string fullPathToFile)
        {
            OpenFile(fullPathToFile);
        }

        /// <summary>
        ///     Get configuration for loaded record
        /// </summary>
        public ConfigurationHandler Configuration { get; private set; }

        //DataFileHandler data;

        internal DataFileHandler Data { get; private set; }

        internal void OpenFile(string fullPathToFile)
        {
            var path = Path.GetDirectoryName(fullPathToFile);
            var filenameWithoutExtention = Path.GetFileNameWithoutExtension(fullPathToFile);
            var extention = Path.GetExtension(fullPathToFile).ToLower();

            if (extention == GlobalSettings.ExtensionCFF) {
                //TODO доделать cff
                throw new NotImplementedException("*.cff not supported");
            }

            if (extention == GlobalSettings.ExtensionCFG || extention == GlobalSettings.ExtensionDAT) {
                Configuration = new ConfigurationHandler(Path.Combine(path, filenameWithoutExtention + ".cfg"));
                Data = new DataFileHandler(Path.Combine(path, filenameWithoutExtention + ".dat"), Configuration);
            }
            else {
                throw new InvalidOperationException("Unsupported file extentions. Must be *.cfg, *.dat, *.cff");
            }
        }

        /// <summary>
        ///     Get common for all channels set of timestamps
        /// </summary>
        /// <returns>In microSeconds</returns>
        public IReadOnlyList<double> GetTimeLine()
        {
            var list = new double[Data.Samples.Length];

            if (Configuration.SamplingRateCount == 0 ||
                Math.Abs(Configuration.SampleRates[0].SamplingFrequency) < 0.01d) {
                //use timestamps in samples
                for (var i = 0; i < Data.Samples.Length; i++) {
                    list[i] = Data.Samples[i].Timestamp * Configuration.TimeMultiplicationFactor;
                }
            }
            else {
                //use calculated by samplingFrequency
                double currentTime = 0;
                var sampleRateIndex = 0;
                const double secondToMicrosecond = 1000000;

                for (var i = 0; i < Data.Samples.Length; i++) {
                    list[i] = currentTime;
                    if (i >= Configuration.SampleRates[sampleRateIndex].LastSampleNumber) sampleRateIndex++;

                    currentTime += secondToMicrosecond / Configuration.SampleRates[sampleRateIndex].SamplingFrequency;
                }
            }

            return list;
        }

        /// <summary>
        ///     Return sequence of values choosen analog channel
        /// </summary>
        public IReadOnlyList<double> GetAnalogPrimaryChannel(int channelNumber)
        {
            double Kt = 1;

            if (Configuration.AnalogChannelInformationList[channelNumber].IsPrimary == false) {
                Kt = Configuration.AnalogChannelInformationList[channelNumber].Primary /
                     Configuration.AnalogChannelInformationList[channelNumber].Secondary;
            }

            var list = new double[Data.Samples.Length];

            for (var i = 0; i < Data.Samples.Length; i++) {
                list[i] = (Data.Samples[i].AnalogValues[channelNumber] *
                           Configuration.AnalogChannelInformationList[channelNumber].MultiplierA +
                           Configuration.AnalogChannelInformationList[channelNumber].MultiplierB) * Kt;
            }

            return list;
        }

        /// <summary>
        ///     Return sequence of values choosen digital channel
        /// </summary>
        public IReadOnlyList<bool> GetDigitalChannel(int channelNumber)
        {
            var list = new bool[Data.Samples.Length];

            for (var i = 0; i < Data.Samples.Length; i++) {
                list[i] = Data.Samples[i].DigitalValues[channelNumber];
            }

            return list;
        }
    }
}
