using System;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of AnalogChannelInformation.
    /// </summary>
    public class AnalogChannelInformation
    {
        int index = 0;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
            internal set
            {
                this.index = value;
            }
        }

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly string name = string.Empty;
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly string phase = string.Empty;
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly string circuitComponent = string.Empty;
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly string units = "NONE";

        public double a = 1.0;
        public double b = 0;
        public readonly double skew = 0;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public double Min { get; internal set; } = float.MinValue;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public double Max { get; internal set; } = float.MaxValue;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly double primary = 1.0;
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly double secondary = 1.0;

        public readonly bool isPrimary = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public AnalogChannelInformation(string name, string phase)
        {
            this.name = name;
            this.phase = phase;
        }

        public AnalogChannelInformation(string analogLine)
        {
            var values = analogLine.Split(GlobalSettings.commaDelimiter);

            this.index = Convert.ToInt32(values[0].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.name = values[1].Trim(GlobalSettings.whiteSpace);
            this.phase = values[2].Trim(GlobalSettings.whiteSpace);
            this.circuitComponent = values[3].Trim(GlobalSettings.whiteSpace);
            this.units = values[4].Trim(GlobalSettings.whiteSpace);
            this.a = Convert.ToDouble(values[5].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.b = Convert.ToDouble(values[6].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.skew = Convert.ToDouble(values[7].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Min = Convert.ToDouble(values[8].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Max = Convert.ToDouble(values[9].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.primary = Convert.ToDouble(values[10].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.secondary = Convert.ToDouble(values[11].Trim(GlobalSettings.whiteSpace), System.Globalization.CultureInfo.InvariantCulture);

            string isPrimaryText = values[12].Trim(GlobalSettings.whiteSpace);
            if (isPrimaryText == "S" || isPrimaryText == "s")
            {
                this.isPrimary = false;
            }

        }

        internal string ToCFGString()
        {
            return this.Index.ToString() + GlobalSettings.commaDelimiter +
                this.name + GlobalSettings.commaDelimiter +
                this.phase + GlobalSettings.commaDelimiter +
                this.circuitComponent + GlobalSettings.commaDelimiter +
                this.units + GlobalSettings.commaDelimiter +
                this.a.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.b.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.skew.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.Min.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.Max.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.primary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                this.secondary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.commaDelimiter +
                (this.isPrimary ? "P" : "S");
        }
    }
}
