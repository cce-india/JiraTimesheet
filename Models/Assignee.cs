using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Assignee
    {
        [JsonProperty("displayName")]
        public string Name { get; set; }
    }
}