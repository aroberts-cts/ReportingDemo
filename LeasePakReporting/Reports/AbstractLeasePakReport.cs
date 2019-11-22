using System;
using System.Collections.Generic;
using ReportingDemo.LeasePakReporting.ReportRows;

namespace ReportingDemo.LeasePakReporting.Reports
{
    public abstract class AbstractLeasePakReport    
    {
        public string ReportFolderPath { get; }
        public IList<ILeasePakReportRow> ReportRows { get; set; }
        public IList<int> ColumnSplitIndices { get; set; }

        protected DateTime ReportDate { get; set; }
        protected string Portfolio { get; set; }
        protected string CachedFilePath { get; set; }

        protected AbstractLeasePakReport()
        {
            ReportFolderPath = ReportFolderPath = RuntimeSettings.LeasePakReporting.ReportingFolderFilepath;
            ReportRows = new List<ILeasePakReportRow>();
            ColumnSplitIndices = new List<int>();
        }
    }
}