namespace Comtrade.Core;

/// <summary>
///     Description of GlobalSettings.
/// </summary>
public static class GlobalSettings
{
    internal const char Comma = ',';
    internal const char WhiteSpace = ' ';
    internal const string DateTimeFormat = "dd/MM/yyyy,HH:mm:ss.ffffff";
    internal const string NewLine = "\r\n";
    internal const string DateTimeFormatForWrite = "dd/MM/yyyy,HH:mm:ss.fffffff";
    internal const string DateTimeFormatForParseMicroSecond = "d/M/yyyy,HH:mm:ss.ffffff";
    internal const string DateTimeFormatForParseNanoSecond = "d/M/yyyy,HH:mm:ss.fffffff";

    /// <summary>
    ///     Extentions of COMTRADE files
    /// </summary>
    public const string ExtensionsForFileDialogFilter = "(*.cfg;*.dat;*.cff)|*.cfg;*.dat;*.cff";

    internal const string ExtensionCFG = ".cfg";
    internal const string ExtensionDAT = ".dat";
    internal const string ExtensionCFF = ".cff";
}
