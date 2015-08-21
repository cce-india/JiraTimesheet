using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Website
    {
        [JsonProperty("value")]
        public string WebSite { get; set; }
    }
}