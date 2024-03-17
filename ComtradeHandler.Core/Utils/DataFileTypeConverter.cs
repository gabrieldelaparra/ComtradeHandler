using System;

namespace ComtradeHandler.Core.Utils;

internal static class DataFileTypeConverter
{
    internal static DataFileType Get(string text)
    {
        text = text.ToLowerInvariant();

        return text switch {
            "ascii" => DataFileType.ASCII,
            "binary" => DataFileType.Binary,
            "binary32" => DataFileType.Binary32,
            "float32" => DataFileType.Float32,
            _ => throw new InvalidOperationException("Undefined *.dat file format")
        };
    }
}
