using System;
using System.Globalization;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of AnalogChannelInformation.
    /// </summary>
    public class AnalogChannelInformation
    {
        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'An'
        /// Analog channel index number
        /// </summary>
        public int Index { get; internal set; } = 0;

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'ch_id'
        /// Channel identifier
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'ph'
        /// Channel phase identification
        /// </summary>
        public string Phase { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'ccbm'
        /// Circuit component being monitored
        /// </summary>
        public string CircuitComponent { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'uu'
        /// Channel units (when data has been converted using channel conversion factor)
        /// </summary>
        public string Units { get; } = "NONE";

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'a'
        /// Channel multiplier (channel conversion factor)
        /// </summary>
        public double MultiplierA { get; set; } = 1.0;

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'b'
        /// Channel offset adder (channel conversion factor)
        /// </summary>
        public double MultiplierB { get; set; }

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'skew'
        /// Channel time skew (in microseconds) from the start of sample period.
        /// </summary>
        public double Skew { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'min'
        /// Channel minimum data range value
        /// </summary>
        public double Min { get; internal set; } = float.MinValue;

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'max'
        /// Channel maximum data range value
        /// </summary>
        public double Max { get; internal set; } = float.MaxValue;

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'primary'
        /// Channel voltage or current transformer ratio primary factor
        /// </summary>
        public readonly double Primary = 1.0;
        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'secondary'
        ///  Channel voltage or current transformer ratio secondary factor
        /// </summary>
        public readonly double Secondary = 1.0;

        /// <summary>
        /// According STD for COMTRADE
        /// Parameter 'PS'
        /// Indicates whether the converted data is in primary (P) or secondary (S) values 
        /// </summary>
        public readonly bool IsPrimary = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnalogChannelInformation(string name, string phase)
        {
            this.Name = name;
            this.Phase = phase;
        }

        public AnalogChannelInformation(string analogLine)
        {
            //TODO: Check if line length == 13;

            var values = analogLine.Split(GlobalSettings.Comma);

            this.Index = Convert.ToInt32(values[0].Trim(), CultureInfo.InvariantCulture);
            this.Name = values[1].Trim();
            this.Phase = values[2].Trim();
            this.CircuitComponent = values[3].Trim();
            this.Units = values[4].Trim();
            this.MultiplierA = Convert.ToDouble(values[5].Trim(), CultureInfo.InvariantCulture);
            this.MultiplierB = Convert.ToDouble(values[6].Trim(), CultureInfo.InvariantCulture);
            this.Skew = Convert.ToDouble(values[7].Trim(), CultureInfo.InvariantCulture);
            this.Min = Convert.ToDouble(values[8].Trim(), CultureInfo.InvariantCulture);
            this.Max = Convert.ToDouble(values[9].Trim(), CultureInfo.InvariantCulture);
            this.Primary = Convert.ToDouble(values[10].Trim(), CultureInfo.InvariantCulture);
            this.Secondary = Convert.ToDouble(values[11].Trim(), CultureInfo.InvariantCulture);

            var isPrimaryText = values[12].Trim();
            if (isPrimaryText.ToLower().Equals("s"))
            {
                this.IsPrimary = false;
            }

        }

        internal string ToCFGString()
        {
            var cfgValues = new[] {
                Index.ToString(),
                Name,
                Phase,
                CircuitComponent,
                Units,
                MultiplierA.ToString(CultureInfo.InvariantCulture),
                MultiplierB.ToString(CultureInfo.InvariantCulture),
                Skew.ToString(CultureInfo.InvariantCulture),
                Min.ToString(CultureInfo.InvariantCulture),
                Max.ToString(CultureInfo.InvariantCulture),
                Primary.ToString(CultureInfo.InvariantCulture),
                Secondary.ToString(CultureInfo.InvariantCulture),
                IsPrimary ? "P" : "S"
            };
            return string.Join(GlobalSettings.Comma.ToString(), cfgValues);
        }
    }
}
