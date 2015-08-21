using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class OriginalAssignee
    {
        [JsonProperty("displayName")]
        public string Originalassignee { get; set; }
    }
}