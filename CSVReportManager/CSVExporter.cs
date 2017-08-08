using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CSVReportManager
{
    public class CSVExporter
    {
        public static void WriteToCSV(List<ReportModel> reportList)
        {
            string attachment = "attachment; filename=report.csv";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");
            WriteColumnName();
            foreach (ReportModel line in reportList)
            {
                WriteReportInfo(line);
            }
            HttpContext.Current.Response.End();
        }

        private static void WriteReportInfo(ReportModel reportItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            AddComma(reportItem.Count.ToString(), stringBuilder);
            AddComma(reportItem.IP.ToString(), stringBuilder);
            HttpContext.Current.Response.Write(stringBuilder.ToString());
            HttpContext.Current.Response.Write(Environment.NewLine);
        }

        private static void AddComma(string value, StringBuilder stringBuilder)
        {
            stringBuilder.Append(value.Replace(',', ' '));
            stringBuilder.Append(", ");
        }

        private static void WriteColumnName()
        {
            string columnNames = "Count, IPAddress";
            HttpContext.Current.Response.Write(columnNames);
            HttpContext.Current.Response.Write(Environment.NewLine);
        }
    }
}
