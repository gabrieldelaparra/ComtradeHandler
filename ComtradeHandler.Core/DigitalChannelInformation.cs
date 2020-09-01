using System;

namespace ComtradeHandler.Core
{
    /// <summary>
    /// Information for digital channel
    /// </summary>
    public class DigitalChannelInformation
    {
        /// <summary>
        /// According STD for COMTRADE
        /// </summary>
        public int Index { get; internal set; }

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
        public bool NormalState { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DigitalChannelInformation(string name, string phase)
        {
            this.Name = name;
            this.Phase = phase;
        }

        public DigitalChannelInformation(string digitalLine)
        {
            var values = digitalLine.Split(GlobalSettings.Comma);

            this.Index = Convert.ToInt32(values[0].Trim(GlobalSettings.WhiteSpace), System.Globalization.CultureInfo.InvariantCulture);
            this.Name = values[1].Trim(GlobalSettings.WhiteSpace);
            this.Phase = values[2].Trim(GlobalSettings.WhiteSpace);
            this.CircuitComponent = values[3].Trim(GlobalSettings.WhiteSpace);
            if (values.Length > 4)
            {//some files not include this part of line
                this.NormalState = Convert.ToBoolean(Convert.ToInt32(values[4].Trim(GlobalSettings.WhiteSpace),
                                                                   System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        internal string ToCFGString()
        {
            return string.Join(GlobalSettings.Comma.ToString(), 
                Index.ToString(), 
                Name,
                Phase, 
                CircuitComponent, 
                NormalState ? "1" : "0");
        }
    }
}
