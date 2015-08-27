using System;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Worklogs
    {
        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        [JsonProperty("started")]
        public DateTimeOffset Started { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }
    }

    public class IssueWorklog
    {
        public int TimeSpentSeconds { get; set; }

        public string TimeSpent { get; set; }

        public string LoggedInPerson { get; set; }
    }

    public class LoggedInUserTimeSpent
    {
        public int TimeSpent { get; set; }

        public string LoggedInPerson { get; set; }
    }
}