namespace PogoVersionMonitor.Configuration
{
    using System.Text.Json.Serialization;

    public class BotConfig
    {
        /// <summary>
        /// Gets or sets the name the Discord bot will use when sending
        /// messages.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the profile icon url the Discord bot will display
        /// when sending messages.
        /// </summary>
        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }
    }
}