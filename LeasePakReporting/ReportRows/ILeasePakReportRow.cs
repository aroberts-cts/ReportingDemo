using System.Collections.Generic;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public interface ILeasePakReportRow
    {
        void SetReportRowText(string rowText, IList<int> columnSplitIndices);

        string GetLeaseNumber();
        decimal GetAmount();
        string GetLesseeNumber();
        string GetLesseeName();

        void SetLesseeName(string lesseeName);

        void PopulateResponse(ILeasePakReportRowResponse response);
    }
}
