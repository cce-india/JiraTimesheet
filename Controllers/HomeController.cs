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

            ViewBag.title = "Displaying " + jiraTimeSheetList.Count + " issues at " + DateTime.Now + " from "+ startDate.Date + " to " + endDate.Date;
            if (jiraTimeSheetList.Count > 0)
            {
                ExportTimeSheetToXlsx(jiraTimeSheetList,GetXlsxFileName(startDate,endDate));
            }
            return View("WorkLogs", jiraTimeSheetList);
        }

        public ActionResult WorkLogs()
        {
            return View();
        }

        //Method to export jira search result to xlsx
        public void ExportTimeSheetToXlsx(List<JiraTimeSheet> jiraTimeSheetList, string xlsxFileName)
        {
            if (jiraTimeSheetList != null && jiraTimeSheetList.Count > 0)
            {
                ExcelUtility excelUtility = new ExcelUtility();
                MemoryStream memoryStream = excelUtility.GetExcel(jiraTimeSheetList.ToDataTable());
                SaveToXlsx(memoryStream, xlsxFileName);
            }
        }

        //Method to download exported jira search result into browser
        public void SaveToXlsx(MemoryStream memoryStream, string xlsxFileName)
        {
            byte[] bytesInStream = memoryStream.ToArray();
            memoryStream.Close();
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + xlsxFileName);
            Response.BinaryWrite(bytesInStream);
            Response.End();
        }

        //Method to generate xlsx file name 
        public string GetXlsxFileName(DateTime startDate, DateTime endDate)
        {
            string fileName = startDate.ToString("MMM") + startDate.ToString("dd") + "-" + endDate.ToString("MMM") + endDate.ToString("dd") + ".xlsx";
            return fileName;
        }
    }
}
