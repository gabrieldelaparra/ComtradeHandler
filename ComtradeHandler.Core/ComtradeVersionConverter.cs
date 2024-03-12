namespace Comtrade.Core
{
    internal static class ComtradeVersionConverter
    {
        internal static ComtradeVersion Get(string versionText)
        {
            switch (versionText) {
                case null:
                    return ComtradeVersion.V1991;
                case "1991":
                    return ComtradeVersion.V1991;
                case "1999":
                    return ComtradeVersion.V1999;
                case "2013":
                    return ComtradeVersion.V2013;
                default:
                    return ComtradeVersion.V1991;
            }
        }
    }
}
