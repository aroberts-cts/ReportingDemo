namespace ReportingDemo.LeasePakReporting.ReportParsers
{
    public abstract class AbstractLeasePakReportParser
    {
        public Constants.LeasePakReporting.Report Report { get; }
        protected AbstractLeasePakReportParser(Constants.LeasePakReporting.Report report)
        {
            Report = report;
        }
    }
}