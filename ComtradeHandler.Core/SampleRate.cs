using System;
using System.Globalization;

namespace Comtrade.Core
{
    /// <summary>
    ///     Description of SampleRate.
    /// </summary>
    public class SampleRate
    {
        public SampleRate(string sampleRateLine)
        {
            var values = sampleRateLine.Split(GlobalSettings.Comma);
            SamplingFrequency = Convert.ToDouble(values[0].Trim(), CultureInfo.InvariantCulture);
            LastSampleNumber = Convert.ToInt32(values[1].Trim());
        }

        /// <summary>
        ///     Hz
        /// </summary>
        public double SamplingFrequency { get; }

        public int LastSampleNumber { get; }
    }
}
