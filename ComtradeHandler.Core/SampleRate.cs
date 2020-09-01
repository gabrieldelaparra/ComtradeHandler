
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
        public double SamplingFrequency { get; }

        public int LastSampleNumber { get; }

        public SampleRate(string sampleRateLine)
        {
            var values = sampleRateLine.Split(GlobalSettings.Comma);
            this.SamplingFrequency = Convert.ToDouble(values[0].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.LastSampleNumber = Convert.ToInt32(values[1].Trim(GlobalSettings.WhiteSpace));
        }
    }
}
