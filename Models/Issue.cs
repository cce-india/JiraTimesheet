using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public class Expand
    {
        [JsonProperty("expand")]
        public string expand { get; set; }

        [JsonProperty("issues")]
        public List<Issue> IssueList { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        [JsonProperty("startAt")]
        public int StartAt { get; set; }
    }
    public class Issue
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }

    }

    public class Fields
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("issuetype")]
        public IssueType IssueType { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset UpdatedDate { get; set; }

        [JsonProperty("worklog")]
        public WorklogOfIssue Worklog { get; set; }

        [JsonProperty("customfield_10101")]
        public OriginalAssignee OriginalAssignee { get; set; }

        [JsonProperty("customfield_10200")]
        public Website Website { get; set; }

        [JsonProperty("reporter")]
        public Reporter Reporter { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset CreatedDate { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }


        [JsonProperty("fixVersions")]
        public List<FixVersion> FixVersions { get; set; }


        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("assignee")]
        public Assignee AssigneeName { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }
    }
}