using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using JiraTimesheet.Models;

namespace JiraTimesheet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            DateTime startDate = Convert.ToDateTime(collection.GetValue("todate").AttemptedValue).Date;
            DateTime endDate = Convert.ToDateTime(collection.GetValue("fromdate").AttemptedValue).Date;
            JiraPresenter jiraPresenter = new JiraPresenter();
            List<JiraTimeSheet> jiraTimeSheetList = jiraPresenter.ProcessIssues(startDate, endDate);
            ViewBag.startdate = startDate;
            ViewBag.enddate = endDate;
            ViewBag.title = "Displaying " + jiraTimeSheetList.Count + " issues at " + DateTime.Now + " from "+ startDate.Date + " to " + endDate.Date;
            return View("WorkLogs", jiraTimeSheetList);
        }

        public ActionResult WorkLogs()
        {
            return View();
        }
    }
}
