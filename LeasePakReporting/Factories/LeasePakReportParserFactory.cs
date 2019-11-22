using ReportingDemo.Factories;
using ReportingDemo.LeasePakReporting.ReportParsers;

namespace ReportingDemo.LeasePakReporting.Factories
{
    public class LeasePakReportParserFactory : AbstractFactory<LeasePakReportParserFactory, ILeasePakReportParser>
    {
        public static T Create<T>() where T : ILeasePakReportParser, new()
        {
            return CreateInstance<T>();
        }
    }
}