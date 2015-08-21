using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Status
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}