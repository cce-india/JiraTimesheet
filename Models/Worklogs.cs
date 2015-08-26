using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Worklogs
    {
        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        [JsonProperty("started")]
        public DateTimeOffset Started { get; set; }
    }
}