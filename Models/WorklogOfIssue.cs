using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class WorklogOfIssue
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("worklogs")]
        public List<Worklogs> Worklogs { get; set; }
    }
}