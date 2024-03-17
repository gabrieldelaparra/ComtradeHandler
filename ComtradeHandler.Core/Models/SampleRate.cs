using System;
using System.Globalization;

namespace ComtradeHandler.Core.Models;

public class SampleRate
{
    public SampleRate(string sampleRateLine)
    {
        var values = sampleRateLine.Split(GlobalSettings.Comma);
        SamplingFrequency = Convert.ToDouble(values[0].Trim(), CultureInfo.InvariantCulture);
        LastSampleNumber = Convert.ToInt32(values[1].Trim());
    }

    public double SamplingFrequency { get; }
    public int LastSampleNumber { get; }
}
