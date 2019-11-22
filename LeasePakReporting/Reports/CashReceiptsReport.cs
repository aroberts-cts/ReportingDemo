using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiFramework.Data.LeasePak;
using ReportingDemo.LeasePakReporting.Factories;
using ReportingDemo.LeasePakReporting.ReportRows;
using ReportingDemo.Repositories.Calendar;
using ReportingDemo.Repositories.File;
using ReportingDemo.Repositories.Lease;

namespace ReportingDemo.LeasePakReporting.Reports
{
    public class CashReceiptsReport : AbstractLeasePakReport, ILeasePakReport
    {
        private ILeaseRepository LeaseRepository { get; }
        private IFileRepository FileRepository { get; }
        private ICalendarRepository CalendarRepository { get; }

        public CashReceiptsReport() : this(new LeaseRepository(), 
            new FileRepository(),
            new CalendarRepository())
        {
        }

        public CashReceiptsReport(ILeaseRepository leaseRepository, 
            IFileRepository fileRepository,
            ICalendarRepository calendarRepository)
        {
            LeaseRepository = leaseRepository;
            FileRepository = fileRepository;
            CalendarRepository = calendarRepository;
        }

        public void SetReportDate(DateTime reportDate)
        {
            ReportDate = reportDate.Date;
        }

        public void SetPortfolio(string portfolio)
        {
            Portfolio = portfolio;
        }

        public void SetColumnSplitIndices(IList<int> columnSplitIndices)
        {
            ColumnSplitIndices = columnSplitIndices;
        }

        public Stream GetFileStream()
        {
            var filePath = GetFilePath();
            if (!FileRepository.FileExists(filePath))
            {
                throw new FileNotFoundException($"The report filepath does not exist: {filePath}");
            }

            return FileRepository.GetFileStream(filePath);
        }

        public string GetFilePath()
        {
            if (string.IsNullOrWhiteSpace(CachedFilePath))
            {
                // Haitham saves the LeasePak reports out on the drive under a folder from two business days ago
                var twoBusinessDaysAgo = CalendarRepository.GetPreviousBusinessDay(CalendarRepository.GetPreviousBusinessDay(ReportDate));
                CachedFilePath = $@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{twoBusinessDaysAgo:MMddyy}\p{Portfolio}_{Constants.LeasePakReporting.CashReceiptsReportFileName}";
            }

            return CachedFilePath;
        }

        public IList<int> GetColumnSplitIndices()
        {
            return ColumnSplitIndices;
        }

        public byte[] GetBytes()
        {
            using (var stream = GetFileStream())
            {
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
        }

        public IList<ILeasePakReportRow> GetDataRows()
        {
            return ReportRows;
        }

        public void AddDataRows(IList<string> reportText)
        {
            if (reportText == null) return;

            var dataRows = reportText.Select(x => LeasePakReportRowFactory.Create<CashReceiptsReportRow>(x, GetColumnSplitIndices())).ToList();
            dataRows.ForEach(x => ReportRows.Add(x));
        }
    }
}