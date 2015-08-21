using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Reporter
    {
        [JsonProperty("displayName")]
        public string reporter { get; set; }
    }
}