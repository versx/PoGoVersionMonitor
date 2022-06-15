namespace PogoVersionMonitor
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using PogoVersionMonitor.Configuration;
    using PogoVersionMonitor.Diagnostics;
    using PogoVersionMonitor.Localization;
    using PogoVersionMonitor.Utilities;

    internal class Program
    {
        static void Main(string[] args)
        {
            var logger = new EventLogger(OnLogEvent);
            var config = Config.Load(Strings.ConfigFileName);
            if (config == null)
            {
                logger.Error($"Failed to load config, exiting...");
                return;
            }

            Translator.Instance.SetLocale(config.Locale);

            var bot = new Bot(config);
            bot.Start();

            logger.Info($"{Strings.BotName} v{Strings.BotVersion} running...");

            Process.GetCurrentProcess().WaitForExit();
            bot.Stop();
        }

        public static void OnLogEvent(LogLevel logLevel, string message)
        {
            // Write log to console
            Console.ForegroundColor = Utils.GetConsoleColor(logLevel);
            var logLevelUpper = logLevel.ToString().ToUpper();
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {logLevelUpper} >> {message}");
            Console.ResetColor();

            try
            {
                // Create logs directory if needed
                Utils.CreateLogsDirectory(Strings.LogsFolder);

                // Write log to file
                var logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                var logPath = Path.Combine(Strings.LogsFolder, logFileName);
                var logData = $"{DateTime.Now.ToLongTimeString()}: {logLevelUpper} >> {message}\r\n";
                File.AppendAllText(logPath, logData);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR]: {ex}");
                Console.ResetColor();
            }
        }
    }
}