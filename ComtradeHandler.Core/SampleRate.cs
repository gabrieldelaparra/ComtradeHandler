﻿
using System;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of SampleRate.
    /// </summary>
    public class SampleRate
    {

        /// <summary>
        /// Hz
        /// </summary>
        public readonly double samplingFrequency = 0;
        public readonly int lastSampleNumber = 0;

        public SampleRate(string sampleRateLine)
        {
            var values = sampleRateLine.Split(GlobalSettings.commaDelimiter);
            this.samplingFrequency = Convert.ToDouble(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.lastSampleNumber = Convert.ToInt32(values[1].Trim(GlobalSettings.whiteSpace));
        }
    }
}
