using System.Collections.Generic;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class SearchRequest
    {
        [JsonProperty("jql")]
        public string Jql { get; set; }

        [JsonProperty("startAt")]
        public int StartAt { get; set; }

        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        [JsonProperty("fields")]
        public List<string> Fields { get; set; }

        public SearchRequest()
        {
            Fields = new List<string>();
        }
    }
}