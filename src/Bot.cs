namespace PogoVersionMonitor
{
    using System;
    using System.Collections.Generic;

    using PogoVersionMonitor.Configuration;
    using PogoVersionMonitor.Diagnostics;
    using PogoVersionMonitor.Localization;
    using PogoVersionMonitor.Net.Webhooks.Models;
    using PogoVersionMonitor.Services;
    using PogoVersionMonitor.Utilities;

    public class Bot
    {
        #region Variables

        private readonly Config _config;
        private readonly VersionMonitor _versionMon;
        private readonly IEventLogger _logger = new EventLogger(Program.OnLogEvent);

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public Bot(Config config)
        {
            _config = config;
            _versionMon = new VersionMonitor(Strings.VersionEndPoint);
            _versionMon.VersionChanged += OnVersionChanged;
            _versionMon.CompareIntervalM = Strings.DefaultCompareIntervalM;
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
            _logger.Info($"{Strings.BotName} v{Strings.BotVersion} started....");
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
            _logger.Info($"{Strings.BotName} v{Strings.BotVersion} stopped....");
        }

        #endregion

        #region Private Methods

        private void OnVersionChanged(object sender, VersionChangedEventArgs e)
        {
            // Generate and build Discord embed message compatible with webhook API
            var eb = GenerateEmbed(e);
            var embed = new DiscordWebhookMessage
            {
                Username = _config.Bot?.Name ?? Strings.BotName,
                AvatarUrl = _config.Bot?.IconUrl ?? Strings.BotIconUrl,
                Embeds = new List<DiscordEmbedMessage> { eb }
            };
            // Convert embed message object to JSON string
            var json = embed.Build();

            // Look all configured webhooks and send embed to each one
            foreach (var webhook in _config.Webhooks)
            {
                _logger.Debug($"Sending embed message to webhook {webhook}");
                NetUtils.SendWebhook(webhook, json);
            }
        }

        private static DiscordEmbedMessage GenerateEmbed(VersionChangedEventArgs e)
        {
            var isLatest = Utils.IsVersionMatch(e.Current, e.Latest);
            var embed = new DiscordEmbedMessage
            {
                Title = Translator.Instance.Translate("embed_title"),
                Fields = new List<DiscordEmbedField>
                {
                    new DiscordEmbedField(Translator.Instance.Translate("embed_field_current"), e.Current.ToString(), true),
                    new DiscordEmbedField(Translator.Instance.Translate("embed_field_latest"), e.Latest.ToString(), true),
                },
                Description = e.IsRevert
                    ? Translator.Instance.Translate("version_reverted")
                    : string.Empty,
                Footer = new DiscordEmbedFooter(isLatest
                    ? Translator.Instance.Translate("embed_footer_api_latest")
                    : Translator.Instance.Translate("embed_footer_api_new"),
                    string.Empty
                ),
                Color = isLatest ? /*GREEN*/ 0x00ff00 : /*RED*/ 0xff0000,
            };
            return embed;
        }

        #endregion
    }
}