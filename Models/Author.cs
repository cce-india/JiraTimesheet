using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Author
    {
        [JsonProperty("displayName")]
        public string Name { get; set; }
    }
}