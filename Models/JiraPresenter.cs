using System;
using System.Collections.Generic;
using System.Linq;

namespace JiraTimesheet.Models
{
    public class JiraPresenter
    {
        // Get all issues names within the given date range
        public List<JiraTimeSheet> ProcessIssues(DateTime startDate, DateTime endDate)
        {
            List<Issue> issueList = new List<Issue>();
            JiraManager manager = new JiraManager();

            int startAt = 0;
            int total;
            // Prepare query to search issues in selected time range
            string jql = "updated >= " + startDate.ToString("yyyy-MM-dd") + " and created <= " + endDate.ToString("yyyy-MM-dd") + " and timespent > 0";
            do
            {
                SearchResponse issueListResponse = manager.GetIssues(jql, startAt: startAt);
                total = issueListResponse.Total;
                startAt = startAt + 50;
                issueList.AddRange(issueListResponse.IssueDescriptions);
            } while (startAt < total);

            var issues = string.Empty;
            foreach (var key in issueList)
            {
                if (!string.IsNullOrEmpty(issues))
                {
                    issues = issues + "," + key.Key;
                }
                else
                {
                    issues = key.Key;
                }
            }

            List<JiraTimeSheet> jiraTimeSheetList = ProcessIssueWorkLogs(issues, startDate, endDate);

            return jiraTimeSheetList;
        }

        // Process issue details and its worlogs
        public List<JiraTimeSheet> ProcessIssueWorkLogs(string issues, DateTime startDate, DateTime endDate)
        {
            List<JiraTimeSheet> jiraTimeSheetList = new List<JiraTimeSheet>();
            JiraManager manager = new JiraManager();

            int startAt = 0;
            int total;
            do
            {
                Expand keyDetailsList = manager.GetIssueDetails(issues, startAt);
                foreach (Issue keyDetails in keyDetailsList.IssueList)
                {
                    //Call jira rest api to get all worklogs for a issue
                    WorklogOfIssue workLogs = manager.GetWorkLogsForIssue(keyDetails.Key);
                    IssueWorklog objWorklog = GetIssueTimeSpent(workLogs, startDate, endDate);
                    if (objWorklog.TimeSpentSeconds > 0)
                    {
                        JiraTimeSheet jiraTimeSheet = new JiraTimeSheet();
                        foreach (FixVersion fixVersion in keyDetails.Fields.FixVersions)
                        {
                            if (!string.IsNullOrEmpty(jiraTimeSheet.FixVersions))
                            {
                                jiraTimeSheet.FixVersions += fixVersion.Version;
                            }
                            else
                            {
                                jiraTimeSheet.FixVersions = fixVersion.Version;
                            }
                        }
                        if (keyDetails.Fields.AssigneeName != null)
                        {
                            jiraTimeSheet.Assignee = keyDetails.Fields.AssigneeName.Name;
                        }
                        if (keyDetails.Fields.OriginalAssignee != null)
                        {
                            jiraTimeSheet.OriginalAssignee = keyDetails.Fields.OriginalAssignee.Originalassignee;
                        }

                        jiraTimeSheet.Created = keyDetails.Fields.CreatedDate.DateTime;
                        jiraTimeSheet.IssueType = keyDetails.Fields.IssueType.Name;
                        jiraTimeSheet.Key = keyDetails.Key;
                        jiraTimeSheet.Project = keyDetails.Fields.Project.Name;
                        jiraTimeSheet.Reporter = keyDetails.Fields.Reporter.reporter;
                        jiraTimeSheet.Status = keyDetails.Fields.Status.Name;
                        jiraTimeSheet.Summary = keyDetails.Fields.Summary;
                        jiraTimeSheet.TimeLoggedBy = objWorklog.LoggedInPerson;
                        jiraTimeSheet.TimeSpent = objWorklog.TimeSpent;
                        jiraTimeSheet.TotalTimeSpent = objWorklog.TimeSpentSeconds;
                        jiraTimeSheet.Updated = keyDetails.Fields.UpdatedDate.DateTime;

                        if (keyDetails.Fields.Website != null)
                        {
                            jiraTimeSheet.Website = keyDetails.Fields.Website.WebSite;
                        }
                        jiraTimeSheetList.Add(jiraTimeSheet);

                    }
                }
                total = keyDetailsList.Total;
                startAt = startAt + 50;
            } while (startAt < total);
            if (jiraTimeSheetList.Count > 0)
            {
                jiraTimeSheetList = jiraTimeSheetList.OrderBy(o => o.Updated).ThenBy(o => o.Created).ThenBy(o => o.FixVersions).ToList();
            }
            return jiraTimeSheetList;
        }

        // Calculate time spent on a issue in given date range
        public IssueWorklog GetIssueTimeSpent(WorklogOfIssue workLogsOfIssue, DateTime toDate, DateTime fromDate)
        {
            int timeSpentSeconds = 0;
            string loggedInPerson = string.Empty;
            string timeSpent = string.Empty;
            IssueWorklog objIssueWorklog = new IssueWorklog();
            List<LoggedInUserTimeSpent> objInUserTimeSpents = new List<LoggedInUserTimeSpent>();
            foreach (Worklogs worklogs in workLogsOfIssue.Worklogs)
            {
                if (worklogs.Started.DateTime >= toDate && worklogs.Started.DateTime <= fromDate)
                {
                    LoggedInUserTimeSpent objUserTimeSpent = new LoggedInUserTimeSpent();
                    if (objInUserTimeSpents.All(o => o.LoggedInPerson != worklogs.Author.Name))
                    {
                        objUserTimeSpent.LoggedInPerson = worklogs.Author.Name;
                        objUserTimeSpent.TimeSpent = worklogs.TimeSpentSeconds;
                        objInUserTimeSpents.Add(objUserTimeSpent);
                    }
                    else
                    {
                        int index = objInUserTimeSpents.FindIndex(objLoggedInUserTimeSpent => objLoggedInUserTimeSpent.LoggedInPerson.Equals(worklogs.Author.Name,StringComparison.Ordinal));
                        objInUserTimeSpents.ElementAt(index).TimeSpent += worklogs.TimeSpentSeconds;
                    }
                    if (timeSpentSeconds > 0)
                    {
                        timeSpentSeconds = timeSpentSeconds + worklogs.TimeSpentSeconds;
                    }
                    else
                    {
                        timeSpentSeconds = worklogs.TimeSpentSeconds;
                    }
                }
            }
            foreach (var loggedInDetails in objInUserTimeSpents)
            {
                if (!string.IsNullOrEmpty(timeSpent))
                {
                    loggedInPerson += "<br/>" + loggedInDetails.LoggedInPerson;
                }
                else
                {
                    loggedInPerson = loggedInDetails.LoggedInPerson;
                }
                if (!string.IsNullOrEmpty(timeSpent))
                {
                    timeSpent += "<br/>" + loggedInDetails.TimeSpent;
                }
                else
                {
                    timeSpent = loggedInDetails.TimeSpent.ToString();
                }
            }
            objIssueWorklog.TimeSpentSeconds = timeSpentSeconds;
            objIssueWorklog.TimeSpent = timeSpent;
            objIssueWorklog.LoggedInPerson = loggedInPerson;

            return objIssueWorklog;
        }
    }
}