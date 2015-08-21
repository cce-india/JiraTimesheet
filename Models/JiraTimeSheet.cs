using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JiraTimesheet.Models
{
    public class JiraTimeSheet
    {
        public string Project { get; set; }

        public string IssueType { get; set; }

        public string Key { get; set; }

        public string Summary { get; set; }

        public string OriginalAssignee { get; set; }

        public string Assignee { get; set; }

        public string Status { get; set; }

        public string FixVersions { get; set; }

        public string Website { get; set; }

        public int TimeSpent { get; set; }

        public DateTime Created { get; set; }

        public string Reporter { get; set; }
        
        public DateTime Updated { get; set; }

     
    }
}