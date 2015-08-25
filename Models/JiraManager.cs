using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace JiraTimesheet.Models
{
    public enum JiraResource
    {
        issue,
        search
    }

    public class JiraManager
    {
        private readonly string _mBaseUrl = ConfigurationManager.AppSettings["JiraRestApiUrl"];
        private readonly string _mUsername = ConfigurationManager.AppSettings["userName"];
        private readonly string _mPassword = ConfigurationManager.AppSettings["password"];

        /// <summary>
        /// Runs a query towards the JIRA REST api
        /// </summary>
        /// <param name="resource">The kind of resource to ask for</param>
        /// <param name="argument">Any argument that needs to be passed, such as a issue</param>
        /// <param name="data">More advanced data sent in POST requests</param>
        /// <param name="method">Either GET or POST</param>
        /// <returns></returns>
        protected string RunQuery(JiraResource resource, string argument = null, string data = null, string method = "GET")
        {
            string result = string.Empty;
            string url = string.Format("{0}{1}/", _mBaseUrl, resource);

            if (argument != null)
            {
                url = string.Format("{0}{1}", url, argument);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = method;

            if (data != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(data);
                }
            }

            string base64Credentials = GetEncodedCredentials();
            request.Headers.Add("Authorization", "Basic " + base64Credentials);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            
            return result;
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", _mUsername, _mPassword);
            byte[] byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        public SearchResponse GetIssues(string jql, List<string> fields = null, int startAt = 0, int maxResult = 50)
        {
            fields = fields ?? new List<string> { "key" };

            SearchRequest request = new SearchRequest
            {
                Fields = fields,
                Jql = jql,
                MaxResults = maxResult,
                StartAt = startAt
            };

            string data = JsonConvert.SerializeObject(request);
            string result = RunQuery(JiraResource.search, data: data, method: "POST");

            SearchResponse response = JsonConvert.DeserializeObject<SearchResponse>(result);

            return response;
        }

        public Expand GetIssueDetails(string issues, int startAt = 0, int maxResults = 50)
        {
            List<string> fields = new List<string> { "issuetype", "project", "key", "summary", "assignee", "status", "fixVersions", "created", "reporter", "updated", "worklog", "customfield_10200", "customfield_10101" };

            string jql = "issue in (" + issues + ")";

            SearchRequest request = new SearchRequest
            {
                Fields = fields,
                Jql = jql,
                MaxResults = maxResults,
                StartAt = startAt
            };

            string data = JsonConvert.SerializeObject(request);

            string result = RunQuery(JiraResource.search, data: data, method: "POST");

            Expand response = JsonConvert.DeserializeObject<Expand>(result);
            return response;
        }
    }
}