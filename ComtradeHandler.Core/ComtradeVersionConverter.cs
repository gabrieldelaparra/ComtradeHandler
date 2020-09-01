namespace ComtradeHandler.Core
{
    internal static class ComtradeVersionConverter
    {
        internal static ComtradeVersion Get(string text)
        {
            if (text == null)
            {
                return ComtradeVersion.V1991;
            }

            if (text == "1991")
            {
                return ComtradeVersion.V1991;
            }

            if (text == "1999")
            {
                return ComtradeVersion.V1999;
            }

            if (text == "2013")
            {
                return ComtradeVersion.V2013;
            }

            return ComtradeVersion.V1991;
        }
    }


}
