namespace PogoVersionMonitor.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json.Serialization;

    using PogoVersionMonitor.Extensions;

    public class Config
    {
        #region Properties

        /// <summary>
        /// Gets or sets the translation locale to use with Discord
        /// embed messages.
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = "en";

        /// <summary>
        /// Gets or sets the bot configuration.
        /// </summary>
        [JsonPropertyName("bot")]
        public BotConfig Bot { get; set; } = new();

        /// <summary>
        /// Gets or sets the webhooks that will receive the version
        /// change messages.
        /// </summary>
        [JsonPropertyName("webhooks")]
        public List<string> Webhooks { get; set; } = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Save the current configuration object
        /// </summary>
        /// <param name="filePath">Path to save the configuration file</param>
        public void Save(string filePath)
        {
            var data = this.ToJson();
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// Load the configuration from a file
        /// </summary>
        /// <param name="filePath">Path to load the configuration file from</param>
        /// <returns>Returns the deserialized configuration object</returns>
        public static Config Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Config not loaded because file not found.", filePath);
            }
            var config = filePath.LoadFromFile<Config>();
            return config;
        }

        #endregion
    }
}