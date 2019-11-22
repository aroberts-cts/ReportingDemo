using System;
using ReportingDemo.Factories;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.Factories
{
    public class LeasePakReportFactory : AbstractFactory<LeasePakReportFactory, ILeasePakReport>
    {
        public static T Create<T>(DateTime reportDate, string portfolio) where T : ILeasePakReport, new()
        {
            var report = CreateInstance<T>();

            report.SetReportDate(reportDate);
            report.SetPortfolio(portfolio);

            return report;
        }
    }
}