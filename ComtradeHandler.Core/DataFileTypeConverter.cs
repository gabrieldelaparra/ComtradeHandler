
using System;

namespace ComtradeHandler.Core
{

    internal static class DataFileTypeConverter
    {
        internal static DataFileType Get(string text)
        {
            text = text.ToLowerInvariant();
            if (text == "ascii")
            {
                return DataFileType.ASCII;
            }

            if (text == "binary")
            {
                return DataFileType.Binary;
            }

            if (text == "binary32")
            {
                return DataFileType.Binary32;
            }

            if (text == "float32")
            {
                return DataFileType.Float32;
            }

            throw new InvalidOperationException("Undefined *.dat file format");
        }
    }


}
