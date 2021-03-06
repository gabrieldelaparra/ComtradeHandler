﻿using System;
using System.Collections.Generic;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Class for parsing comtrade files
    /// </summary>
    public class RecordReader
    {
        /// <summary>
        /// Get configuration for loaded record
        /// </summary>
        public ConfigurationHandler Configuration
        {
            get;
            private set;
        }

        //DataFileHandler data;

        internal DataFileHandler Data
        {
            get;
            private set;
        }

        internal RecordReader()
        {
        }

        /// <summary>
        /// Read record from file
        /// </summary>
        public RecordReader(string fullPathToFile)
        {
            this.OpenFile(fullPathToFile);
        }

        internal void OpenFile(string fullPathToFile)
        {
            string path = System.IO.Path.GetDirectoryName(fullPathToFile);
            string filenameWithoutExtention = System.IO.Path.GetFileNameWithoutExtension(fullPathToFile);
            string extention = System.IO.Path.GetExtension(fullPathToFile).ToLower();

            if (extention == GlobalSettings.ExtensionCFF)
            {
                //TODO доделать cff
                throw new NotImplementedException("*.cff not supported");
            }
            else if (extention == GlobalSettings.ExtensionCFG || extention == GlobalSettings.ExtensionDAT)
            {
                this.Configuration = new ConfigurationHandler(System.IO.Path.Combine(path, filenameWithoutExtention + ".cfg"));
                this.Data = new DataFileHandler(System.IO.Path.Combine(path, filenameWithoutExtention + ".dat"), this.Configuration);
            }
            else
            {
                throw new InvalidOperationException("Unsupported file extentions. Must be *.cfg, *.dat, *.cff");
            }
        }

        /// <summary>
        /// Get common for all channels set of timestamps
        /// </summary>
        /// <returns>In microSeconds</returns>
        public IReadOnlyList<double> GetTimeLine()
        {
            var list = new double[this.Data.Samples.Length];

            if (this.Configuration.SamplingRateCount == 0 ||
               (Math.Abs(this.Configuration.SampleRates[0].SamplingFrequency) < 0.01d))
            {
                //use timestamps in samples
                for (int i = 0; i < this.Data.Samples.Length; i++)
                {
                    list[i] = this.Data.Samples[i].Timestamp * this.Configuration.TimeMultiplicationFactor;
                }
            }
            else
            {//use calculated by samplingFrequency
                double currentTime = 0;
                int sampleRateIndex = 0;
                const double secondToMicrosecond = 1000000;
                for (int i = 0; i < this.Data.Samples.Length; i++)
                {
                    list[i] = currentTime;
                    if (i >= this.Configuration.SampleRates[sampleRateIndex].LastSampleNumber)
                    {
                        sampleRateIndex++;
                    }

                    currentTime += secondToMicrosecond / this.Configuration.SampleRates[sampleRateIndex].SamplingFrequency;
                }
            }

            return list;
        }

        /// <summary>
        /// Return sequence of values choosen analog channel
        /// </summary>
        public IReadOnlyList<double> GetAnalogPrimaryChannel(int channelNumber)
        {
            double Kt = 1;
            if (this.Configuration.AnalogChannelInformationList[channelNumber].IsPrimary == false)
            {
                Kt = this.Configuration.AnalogChannelInformationList[channelNumber].Primary /
                    this.Configuration.AnalogChannelInformationList[channelNumber].Secondary;
            }

            var list = new double[this.Data.Samples.Length];
            for (int i = 0; i < this.Data.Samples.Length; i++)
            {
                list[i] = (this.Data.Samples[i].AnalogValues[channelNumber] * this.Configuration.AnalogChannelInformationList[channelNumber].MultiplierA +
                         this.Configuration.AnalogChannelInformationList[channelNumber].MultiplierB) * Kt;
            }
            return list;
        }

        /// <summary>
        /// Return sequence of values choosen digital channel
        /// </summary>
        public IReadOnlyList<bool> GetDigitalChannel(int channelNumber)
        {
            var list = new bool[this.Data.Samples.Length];
            for (int i = 0; i < this.Data.Samples.Length; i++)
            {
                list[i] = this.Data.Samples[i].DigitalValues[channelNumber];
            }
            return list;
        }
    }
}
