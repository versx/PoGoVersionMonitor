namespace PogoVersionMonitor
{
    using System.IO;

    public static class Strings
    {
        public const string BotName = "PoGo Version Monitor";

        public static readonly string BotVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public const string BotIconUrl = "https://w7.pngwing.com/pngs/652/369/png-transparent-pokemon-go-computer-icons-raid-niantic-pokemongo-video-game-boss-pokemon.png";

        public const string BasePath = "../bin/";

        public const string ConfigFileName = "config.json";

        public const string LogsFolderName = "logs";

        public static readonly string LogsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            LogsFolderName
        );

        public const string StaticFolder = "static";

        public static readonly string LocaleFolder = StaticFolder + Path.DirectorySeparatorChar + "locales";

        public const string VersionEndPoint = "https://pgorelease.nianticlabs.com/plfe/version";
    }
}