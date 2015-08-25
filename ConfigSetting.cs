using System.Configuration;


namespace JiraTimesheet
{
    public static class ConfigSetting
    {

        private static string _jiraRestApiUrl;
        private static string _jiraUsername;
        private static string _jiraPassword;
        private static string _sBaseUrl;
        private static string _sJiraUrl;

        public static string JiraRestApiUrl
        {
            get { return _jiraRestApiUrl ?? (_jiraRestApiUrl = ConfigurationManager.AppSettings["JiraRestApiUrl"]); }
        }

        public static string JiraUsername
        {
            get { return _jiraUsername ?? (_jiraUsername = ConfigurationManager.AppSettings["userName"]); }
        }


        public static string JiraPassword
        {
            get { return _jiraPassword ?? (_jiraPassword = ConfigurationManager.AppSettings["password"]); }
        }


        public static string BaseUrl
        {
            get { return _sBaseUrl ?? (_sBaseUrl = ConfigurationManager.AppSettings["BaseUrl"]); }
        }

        public static string JiraUrl
        {
            get { return _sJiraUrl ?? (_sJiraUrl = ConfigurationManager.AppSettings["JiraBaseUrl"]); }
        }
    }
}