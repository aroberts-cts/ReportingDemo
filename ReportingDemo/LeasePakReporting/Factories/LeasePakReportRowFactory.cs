using System.Collections.Generic;
using ReportingDemo.Factories;
using ReportingDemo.LeasePakReporting.ReportRows;

namespace ReportingDemo.LeasePakReporting.Factories
{
    public class LeasePakReportRowFactory : AbstractFactory<LeasePakReportRowFactory, ILeasePakReportRow>
    {
        public static T Create<T>(string reportRowText, IList<int> columnSplitIndices) where T : ILeasePakReportRow, new()
        {
            var reportRow = CreateInstance<T>();
            //Set fields from inside report
            reportRow.SetReportRowText(reportRowText, columnSplitIndices);

            return reportRow;
        }
    }
}