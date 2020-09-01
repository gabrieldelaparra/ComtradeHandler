using System;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Description of AnalogChannelInformation.
    /// </summary>
    public class AnalogChannelInformation
    {
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public int Index { get; internal set; } = 0;

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string Phase { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string CircuitComponent { get; }

        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public string Units { get; } = "NONE";

        public double a = 1.0;
        public double b = 0;
        public double Skew { get; } = 0;

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
        public readonly double Primary = 1.0;
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public readonly double Secondary = 1.0;

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

            this.Index = Convert.ToInt32(values[0].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Name = values[1].Trim(GlobalSettings.WhiteSpace);
            this.Phase = values[2].Trim(GlobalSettings.WhiteSpace);
            this.CircuitComponent = values[3].Trim(GlobalSettings.WhiteSpace);
            this.Units = values[4].Trim(GlobalSettings.WhiteSpace);
            this.a = Convert.ToDouble(values[5].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.b = Convert.ToDouble(values[6].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Skew = Convert.ToDouble(values[7].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Min = Convert.ToDouble(values[8].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Max = Convert.ToDouble(values[9].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Primary = Convert.ToDouble(values[10].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Secondary = Convert.ToDouble(values[11].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);

            var isPrimaryText = values[12].Trim(GlobalSettings.WhiteSpace);
            if (isPrimaryText == "S" || isPrimaryText == "s")
            {
                this.IsPrimary = false;
            }

        }

        internal string ToCFGString()
        {
            return this.Index.ToString() + GlobalSettings.Comma +
                this.Name + GlobalSettings.Comma +
                this.Phase + GlobalSettings.Comma +
                this.CircuitComponent + GlobalSettings.Comma +
                this.Units + GlobalSettings.Comma +
                this.a.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.b.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.Skew.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.Min.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.Max.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.Primary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                this.Secondary.ToString(System.Globalization.CultureInfo.InvariantCulture) + GlobalSettings.Comma +
                (this.IsPrimary ? "P" : "S");
        }
    }
}
