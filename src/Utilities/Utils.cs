namespace PogoVersionMonitor.Utilities
{
    using System;
    using System.IO;
    using System.Net;

    using PogoVersionMonitor.Diagnostics;

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

        public static void CreateLogsDirectory(string folderPath)
        {
            // Create logs folder if not exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static ConsoleColor GetConsoleColor(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Error => ConsoleColor.DarkRed,
                LogLevel.Info => ConsoleColor.White,
                LogLevel.Success => ConsoleColor.Green,
                LogLevel.Trace => ConsoleColor.Cyan,
                LogLevel.Warning => ConsoleColor.Yellow,
                _ => ConsoleColor.DarkGray,
            };
        }
    }
}