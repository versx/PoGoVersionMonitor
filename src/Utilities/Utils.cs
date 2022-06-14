namespace PogoVersionMonitor.Utilities
{
    using System;
    using System.Net;

    internal class Utils
    {
        public static bool IsVersionMatch(Version current, Version latest)
        {
            return
                current.Major == latest.Major &&
                current.Minor == latest.Minor &&
                current.Build == latest.Build;
        }

        public static string GetRequest(string url)
        {
            using var wc = new WebClient();
            wc.Proxy = null;
            var data = wc.DownloadString(url);
            return data;
        }

        public static string SanitizeString(string version)
        {
            return version.Trim('\0', '\r', '\n');
        }
    }
}