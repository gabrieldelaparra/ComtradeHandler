using System;

namespace Comtrade.Core
{
    internal static class DataFileTypeConverter
    {
        internal static DataFileType Get(string text)
        {
            text = text.ToLowerInvariant();

            switch (text) {
                case "ascii":
                    return DataFileType.ASCII;
                case "binary":
                    return DataFileType.Binary;
                case "binary32":
                    return DataFileType.Binary32;
                case "float32":
                    return DataFileType.Float32;
                default:
                    throw new InvalidOperationException("Undefined *.dat file format");
            }
        }
    }
}
