namespace PogoVersionMonitor.Net.Webhooks.Models
{
    using System.Text.Json.Serialization;

    public class DiscordEmbedField
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("inline")]
        public bool Inline { get; set; }

        public DiscordEmbedField()
        {
        }

        public DiscordEmbedField(string name, string value, bool inline = false)
        {
            Name = name;
            Value = value;
            Inline = inline;
        }
    }
}