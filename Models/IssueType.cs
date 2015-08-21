using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class IssueType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}