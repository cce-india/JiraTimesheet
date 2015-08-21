using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class FixVersion
    {
        [JsonProperty("name")]
        public string Version { get; set; }
    }
}