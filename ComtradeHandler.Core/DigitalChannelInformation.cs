using System;
using System.Globalization;

namespace Comtrade.Core
{
    /// <summary>
    ///     Information for digital channel
    /// </summary>
    public class DigitalChannelInformation
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DigitalChannelInformation(string name, string phase)
        {
            Name = name;
            Phase = phase;
        }

        public DigitalChannelInformation(string digitalLine)
        {
            var values = digitalLine.Split(GlobalSettings.Comma);

            Index = Convert.ToInt32(values[0].Trim(), CultureInfo.InvariantCulture);
            Name = values[1].Trim();
            Phase = values[2].Trim();
            CircuitComponent = values[3].Trim();
            if (values.Length > 4)
                //some files not include this part of line
                NormalState = Convert.ToBoolean(Convert.ToInt32(values[4].Trim(), CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     According STD for COMTRADE
        ///     Parameter 'Dn'
        ///     Status channel index number
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        ///     According STD for COMTRADE
        ///     Parameter 'ch_id'
        ///     Channel identifier
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     According STD for COMTRADE
        ///     Parameter 'ph'
        ///     Channel phase identification
        /// </summary>
        public string Phase { get; }

        /// <summary>
        ///     According STD for COMTRADE
        ///     Parameter 'ccbm'
        ///     Circuit component being monitored
        /// </summary>
        public string CircuitComponent { get; }

        /// <summary>
        ///     According STD for COMTRADE
        ///     Parameter 'y'
        ///     Normal (steady state operation) state of status channel
        /// </summary>
        public bool NormalState { get; }

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