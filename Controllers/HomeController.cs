using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CSVReportManager;
using Tx.Windows;

namespace ShelleyBenhoff.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public EmptyResult Report()
        {
            var logfilePath = HttpContext.Server.MapPath("~/App_Data/access.log");
            var iisLog = W3CEnumerable.FromFile(logfilePath);
            List<ReportModel> report = new List<ReportModel>();
            
            foreach (var line in iisLog.Where(x => !string.IsNullOrEmpty(x.c_ip) && !x.c_ip.Contains("207.114") && x.cs_method == "GET")
                .GroupBy(x => x.c_ip)        
                .Select(group => new {
                    Counter = group.Count(),
                    IP = group.Key
                })
                .OrderByDescending(x => x.Counter)
                .GroupBy(x => x.Counter)
                .OrderByDescending(x => x.IP))
            {
                ReportModel reportItem = new ReportModel();
                reportItem.Count = line.Counter;
                reportItem.IP = IPAddress.Parse(line.IP);
                report.Add(reportItem);
            }

            CSVExporter.WriteToCSV(report);

            return new EmptyResult();
        }
    }
}