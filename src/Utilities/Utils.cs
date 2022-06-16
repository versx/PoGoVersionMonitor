namespace PogoVersionMonitor.Utilities
{
    using System;
    using System.IO;
    using System.Net;

    using PogoVersionMonitor.Diagnostics;

    internal class Utils
    {
        /// <summary>
        /// Compares two <see cref="Version"/> objects against each other to see if
        /// they are the same numerical version.
        /// </summary>
        /// <param name="current">Current version to compare against.</param>
        /// <param name="latest">Latest version to compare against.</param>
        /// <returns></returns>
        public static bool IsVersionMatch(Version current, Version latest)
        {
            return
                current.Major == latest.Major &&
                current.Minor == latest.Minor &&
                current.Build == latest.Build;
        }

        /// <summary>
        /// Performs a GET HTTP web request to the provided source.
        /// </summary>
        /// <param name="url">Web address to perform GET HTTP request to.</param>
        /// <returns>Returns the raw unaltered data fetched from the provided source.</returns>
        public static string GetRequest(string url)
        {
            using var wc = new WebClient();
            wc.Proxy = null;
            var data = wc.DownloadString(url);
            return data;
        }

        /// <summary>
        /// Sanitizes the provided data by removing any null characters, carriage return,
        /// new line, or bell/alarm escaped characters.
        /// </summary>
        /// <param name="version">String to sanitize and clean.</param>
        /// <returns>Returns the provided string sanitized.</returns>
        public static string SanitizeString(string data)
        {
            return data.Trim('\0', '\r', '\n', '\a');
        }

        /// <summary>
        /// Creates directory at provided path if not already existing.
        /// </summary>
        /// <param name="folderPath">Relative or absolute folder path to create.</param>
        public static void CreateDirectory(string folderPath)
        {
            // Create folder if not already exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        /// <summary>
        /// Gets the associated console color related to the logging level provided.
        /// </summary>
        /// <param name="logLevel">Log leve to get console color for.</param>
        /// <returns>Returns the console color related to the log level.</returns>
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