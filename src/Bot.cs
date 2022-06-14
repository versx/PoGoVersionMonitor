namespace PogoVersionMonitor
{
    using System;
    using System.Collections.Generic;

    using PogoVersionMonitor.Configuration;
    using PogoVersionMonitor.Net.Webhooks.Models;
    using PogoVersionMonitor.Services;
    using PogoVersionMonitor.Utilities;

    public class Bot
    {
        #region Variables

        private readonly Config _config;
        private readonly VersionMonitor _versionMon;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public Bot(Config config)
        {
            _config = config;
            _versionMon = new VersionMonitor();
            _versionMon.VersionChanged += OnVersionChanged;
            _versionMon.CompareIntervalM = 1;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the version monitor service.
        /// </summary>
        public void Start()
        {
            // Skip starting if already started
            if (_versionMon.IsRunning)
                return;

            _versionMon.Start();
            Console.WriteLine($"{Strings.BotName} v{Strings.BotVersion} started....");
        }

        /// <summary>
        /// Stop the version monitor service.
        /// </summary>
        public void Stop()
        {
            // Skip stopping if already stopped
            if (!_versionMon.IsRunning)
                return;

            _versionMon.Stop();
            Console.WriteLine($"{Strings.BotName} v{Strings.BotVersion} stopped....");
        }

        #endregion

        #region Private Methods

        private void OnVersionChanged(object sender, VersionChangedEventArgs e)
        {
            Console.WriteLine($"Latest version changed from {e.Current} -> {e.Latest}");

            var eb = GenerateEmbed(e);
            var embed = new DiscordWebhookMessage
            {
                Username = _config.Bot?.Name ?? Strings.BotName,
                AvatarUrl = _config.Bot?.IconUrl ?? Strings.BotIconUrl,
                Embeds = new List<DiscordEmbedMessage> { eb }
            };
            var json = embed.Build();

            foreach (var webhook in _config.Webhooks)
            {
                Console.WriteLine($"Sending embed message to webhook {webhook}");
                NetUtils.SendWebhook(webhook, json);
            }
        }

        private static DiscordEmbedMessage GenerateEmbed(VersionChangedEventArgs e)
        {
            var isLatest = Utils.IsVersionMatch(e.Current, e.Latest);
            var embed = new DiscordEmbedMessage
            {
                Title = "Pokemon Go Version Monitor:",
                Fields = new List<DiscordEmbedField>
                {
                    new DiscordEmbedField("Current:", e.Current.ToString(), true),
                    new DiscordEmbedField("Latest:", e.Latest.ToString(), true),
                },
                Description = e.IsRevert
                    ? "Previous Version Reverted!"
                    : string.Empty,
                Footer = new DiscordEmbedFooter(isLatest
                    ? "LATEST API VERSION"
                    : "NEW API RELEASED",
                    string.Empty
                ),
                Color = isLatest ? /*GREEN*/ 0x00ff00 : /*RED*/ 0xff0000,
            };
            return embed;
        }

        #endregion
    }
}