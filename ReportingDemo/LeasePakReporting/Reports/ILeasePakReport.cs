using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using ReportingDemo.LeasePakReporting.ReportRows;

namespace ReportingDemo.LeasePakReporting.Reports
{
    [ServiceContract]
    public interface ILeasePakReport
    {
        void SetReportDate(DateTime reportDate);
        void SetPortfolio(string portfolio);
        void SetColumnSplitIndices(IList<int> columnSplitIndices);

        Stream GetFileStream();
        string GetFilePath();
        IList<int> GetColumnSplitIndices();

        [OperationContract]
        byte[] GetBytes();
        [OperationContract]
        IList<ILeasePakReportRow> GetDataRows();

        void AddDataRows(IList<string> reportText);
    }
}
