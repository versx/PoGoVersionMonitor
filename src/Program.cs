namespace PogoVersionMonitor
{
    using System;
    using System.Diagnostics;

    using PogoVersionMonitor.Configuration;

    // TODO: Localize embed text
    // TODO: Add EventLogger and logs

    internal class Program
    {
        static void Main(string[] args)
        {
            var config = Config.Load(Strings.ConfigFileName);
            if (config == null)
            {
                Console.WriteLine($"Failed to load config, exiting...");
                return;
            }

            var bot = new Bot(config);
            bot.Start();

            Process.GetCurrentProcess().WaitForExit();
            bot.Stop();
        }
    }
}