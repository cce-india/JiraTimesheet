using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Project
    {
        [JsonProperty("key")]
        public string Name { get; set; }
    }
}