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
using ReportingDemo.Repositories.Lessee;

namespace ReportingDemo.LeasePakReporting.Reports
{
    public class PreAuthorizedPaymentReport : AbstractLeasePakReport, ILeasePakReport
    {
        private ILeaseRepository LeaseRepository { get; }
        private ILesseeRepository LesseeRepository { get; }
        private ICalendarRepository CalendarRepository { get; }
        private IFileRepository FileRepository { get; }

        public PreAuthorizedPaymentReport() : this(new LeaseRepository(), 
            new LesseeRepository(),
            new FileRepository(),
            new CalendarRepository())
        {
        }

        public PreAuthorizedPaymentReport(ILeaseRepository leaseRepository,
            ILesseeRepository lesseeRepository,
            IFileRepository fileRepository,
            ICalendarRepository calendarRepository)
        {
            LeaseRepository = leaseRepository;
            LesseeRepository = lesseeRepository;
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

        public string GetFilePath()
        {
            if (string.IsNullOrWhiteSpace(CachedFilePath))
            {
                // Haitham saves the LeasePak reports out on the drive under a folder from two business days ago
                var twoBusinessDaysAgo = CalendarRepository.GetPreviousBusinessDay(CalendarRepository.GetPreviousBusinessDay(ReportDate));
                CachedFilePath = $@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{twoBusinessDaysAgo:MMddyy}\p{Portfolio}_{Constants.LeasePakReporting.PreAuthorizedPaymentsReportFileName}";
            }

            return CachedFilePath;
        }

        public IList<ILeasePakReportRow> GetDataRows()
        {
            return ReportRows;
        }

        public void AddDataRows(IList<string> parsedText)
        {
            var dataRows = parsedText.Select(x => LeasePakReportRowFactory.Create<PreAuthorizedPaymentReportRow>(x, GetColumnSplitIndices())).ToList();
            foreach (var row in dataRows)
            {
                var corporateCostCenter = LeaseRepository.GetCorporateCostCenterByLeaseNumber(row.GetLeaseNumber());
                row.SetCorporateCostCenter(corporateCostCenter);

                var lesseeName = LesseeRepository.GetLesseeNameFromLesseeNumber(row.GetLesseeNumber());
                row.SetLesseeName(lesseeName);
            }

            dataRows.ForEach(x => ReportRows.Add(x));
        }
    }
}