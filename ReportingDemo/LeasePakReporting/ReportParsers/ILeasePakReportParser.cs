using System;
using System.IO;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.ReportParsers
{
    public interface ILeasePakReportParser
    {
        ILeasePakReport Parse(DateTime reportDate, string portfolio);
        void SkipReportHeaderData(StreamReader reportStream);

        string GetFirstColumnName();
    }
}
