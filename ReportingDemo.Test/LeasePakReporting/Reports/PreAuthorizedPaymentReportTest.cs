using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFramework.Data.LeasePak.Domain;
using Moq;
using ReportingDemo.LeasePakReporting.ReportRows;
using ReportingDemo.LeasePakReporting.Reports;
using ReportingDemo.Repositories.Calendar;
using ReportingDemo.Repositories.File;
using ReportingDemo.Repositories.Lease;
using ReportingDemo.Repositories.Lessee;

namespace ReportingDemo.Test.LeasePakReporting.Reports
{
    [TestClass]
    public class PreAuthorizedPaymentReportTest
    {
        #region GetFilePath

        [TestMethod]
        public void GetFilePath_HappyPath()
        {
            var mock = new Mock<ICalendarRepository>();
            mock.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var portfolio = "A";
            var obj = new PreAuthorizedPaymentReport(null, null, null, mock.Object);
            obj.SetPortfolio(portfolio);
            obj.SetReportDate(DateTime.Today);

            var filePath = obj.GetFilePath();

            // Have to get two business days ago
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            Assert.AreEqual($@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{DateTime.Today:MMddyy}\p{portfolio}_{Constants.LeasePakReporting.PreAuthorizedPaymentsReportFileName}", filePath);
        }

        [TestMethod]
        public void GetFilePath_UseCachedPath()
        {
            var mock = new Mock<ICalendarRepository>();
            mock.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var portfolio = "A";
            var obj = new PreAuthorizedPaymentReport(null, null, null, mock.Object);
            obj.SetPortfolio(portfolio);
            obj.SetReportDate(DateTime.Today);

            obj.GetFilePath();

            // Have to get two business days ago
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            var filePath = obj.GetFilePath();

            // Make sure it didn't call out to the function again
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            Assert.AreEqual($@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{DateTime.Today:MMddyy}\p{portfolio}_{Constants.LeasePakReporting.PreAuthorizedPaymentsReportFileName}", filePath);
        }

        #endregion

        #region GetFileStream

        [TestMethod]
        public void GetFileStream_HappyPath()
        {
            var mockFile = new Mock<IFileRepository>();
            mockFile.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            var mockCalendar = new Mock<ICalendarRepository>();
            mockCalendar.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var obj = new PreAuthorizedPaymentReport(null, null, mockFile.Object, mockCalendar.Object);
            obj.SetPortfolio("A");
            obj.SetReportDate(DateTime.Today);

            var fileStream = obj.GetFileStream();

            mockFile.Verify(x => x.FileExists(obj.GetFilePath()));
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileStream_FileDoesNotExist()
        {
            var mockFile = new Mock<IFileRepository>();
            mockFile.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var mockCalendar = new Mock<ICalendarRepository>();
            mockCalendar.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var obj = new PreAuthorizedPaymentReport(null, null, mockFile.Object, mockCalendar.Object);
            obj.SetPortfolio("A");
            obj.SetReportDate(DateTime.Today);

            obj.GetFileStream();

            Assert.Fail();
        }

        #endregion

        #region GetBytes

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetBytes_FileDoesNotExist()
        {
            var mockFile = new Mock<IFileRepository>();
            mockFile.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            var mockCalendar = new Mock<ICalendarRepository>();
            mockCalendar.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var obj = new PreAuthorizedPaymentReport(null, null, mockFile.Object, mockCalendar.Object);
            obj.SetPortfolio("A");
            obj.SetReportDate(DateTime.Today);

            obj.GetBytes();

            Assert.Fail();
        }

        #endregion

        #region AddDataRows

        [TestMethod]
        public void AddDataRows_HappyPath()
        {
            var lease = "B";
            var lessee = "C";
            var instanceId = "E";
            var payerAccount = "F";
            var invoiceNumber = "G";
            var dueDate = new DateTime(2000, 1, 1);
            var amount = 1000.00m;

            var reportLine = $"A {lease} {lessee} D {instanceId} {payerAccount} {invoiceNumber} {dueDate:MM/dd/yyyy} {amount}";
            var parsedText = new List<string>() { reportLine, reportLine, reportLine };

            var corporateCostCenter = "Y";
            var mockLeaseRepository = new Mock<ILeaseRepository>();
            mockLeaseRepository.Setup(x => x.GetLeaseByLeaseNumber(It.IsAny<string>())).Returns(new rls());
            mockLeaseRepository.Setup(x => x.GetCorporateCostCenterByLeaseNumber(It.IsAny<string>())).Returns(corporateCostCenter);

            var lesseeName = "Z";
            var mockLesseeRepository = new Mock<ILesseeRepository>();
            mockLesseeRepository.Setup(x => x.GetLesseeNameFromLesseeNumber(It.IsAny<string>())).Returns(lesseeName);

            var obj = new PreAuthorizedPaymentReport(mockLeaseRepository.Object, mockLesseeRepository.Object, null, null);
            obj.SetColumnSplitIndices(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 24 });
            obj.AddDataRows(parsedText);

            var dataRows = obj.GetDataRows();

            Assert.IsTrue(dataRows.Any());
            Assert.AreEqual(parsedText.Count, dataRows.Count);
            Assert.IsTrue(dataRows.All(x => x != null));
            Assert.IsTrue(dataRows.All(x => ((PreAuthorizedPaymentReportRow) x).GetCorporateCostCenter().Equals(corporateCostCenter)));
            Assert.IsTrue(dataRows.All(x => ((PreAuthorizedPaymentReportRow) x).GetLesseeName().Equals(lesseeName)));
        }

        [TestMethod]
        public void AddDataRows_EmptyList()
        {
            var parsedText = new List<string>();

            var obj = new CashReceiptsReport(null, null, null);
            obj.AddDataRows(parsedText);

            Assert.IsTrue(!obj.GetDataRows().Any());
        }

        [TestMethod]
        public void AddDataRows_NullList()
        {
            var obj = new CashReceiptsReport(null, null, null);
            obj.AddDataRows(null);

            Assert.IsTrue(!obj.GetDataRows().Any());
        }

        [TestMethod]
        public void AddDataRows_NoCorporateCostCenter()
        {
            var lease = "B";
            var lessee = "C";
            var instanceId = "E";
            var payerAccount = "F";
            var invoiceNumber = "G";
            var dueDate = new DateTime(2000, 1, 1);
            var amount = 1000.00m;

            var reportLine = $"A {lease} {lessee} D {instanceId} {payerAccount} {invoiceNumber} {dueDate:MM/dd/yyyy} {amount}";
            var parsedText = new List<string>() { reportLine };

            var mockLeaseRepository = new Mock<ILeaseRepository>();
            mockLeaseRepository.Setup(x => x.GetLeaseByLeaseNumber(It.IsAny<string>())).Returns(new rls());
            mockLeaseRepository.Setup(x => x.GetCorporateCostCenterByLeaseNumber(It.IsAny<string>()));

            var lesseeName = "Z";
            var mockLesseeRepository = new Mock<ILesseeRepository>();
            mockLesseeRepository.Setup(x => x.GetLesseeNameFromLesseeNumber(It.IsAny<string>())).Returns(lesseeName);

            var obj = new PreAuthorizedPaymentReport(mockLeaseRepository.Object, mockLesseeRepository.Object, null, null);
            obj.SetColumnSplitIndices(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 24 });
            obj.AddDataRows(parsedText);

            var dataRows = obj.GetDataRows();

            Assert.IsTrue(dataRows.Any());
            Assert.AreEqual(parsedText.Count, dataRows.Count);
            Assert.IsTrue(dataRows.All(x => string.IsNullOrWhiteSpace(((PreAuthorizedPaymentReportRow)x).GetCorporateCostCenter())));
            Assert.IsTrue(dataRows.All(x => ((PreAuthorizedPaymentReportRow)x).GetLesseeName().Equals(lesseeName)));
        }

        [TestMethod]
        public void AddDataRows_NoLesseeName()
        {
            var lease = "B";
            var lessee = "C";
            var instanceId = "E";
            var payerAccount = "F";
            var invoiceNumber = "G";
            var dueDate = new DateTime(2000, 1, 1);
            var amount = 1000.00m;

            var reportLine = $"A {lease} {lessee} D {instanceId} {payerAccount} {invoiceNumber} {dueDate:MM/dd/yyyy} {amount}";
            var parsedText = new List<string>() { reportLine };

            var corporateCostCenter = "Y";
            var mockLeaseRepository = new Mock<ILeaseRepository>();
            mockLeaseRepository.Setup(x => x.GetLeaseByLeaseNumber(It.IsAny<string>())).Returns(new rls());
            mockLeaseRepository.Setup(x => x.GetCorporateCostCenterByLeaseNumber(It.IsAny<string>())).Returns(corporateCostCenter);

            var mockLesseeRepository = new Mock<ILesseeRepository>();
            mockLesseeRepository.Setup(x => x.GetLesseeNameFromLesseeNumber(It.IsAny<string>()));

            var obj = new PreAuthorizedPaymentReport(mockLeaseRepository.Object, mockLesseeRepository.Object, null, null);
            obj.SetColumnSplitIndices(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 24 });
            obj.AddDataRows(parsedText);

            var dataRows = obj.GetDataRows();

            Assert.IsTrue(dataRows.Any());
            Assert.AreEqual(parsedText.Count, dataRows.Count);
            Assert.IsTrue(dataRows.All(x => ((PreAuthorizedPaymentReportRow)x).GetCorporateCostCenter().Equals(corporateCostCenter)));
            Assert.IsTrue(dataRows.All(x => string.IsNullOrWhiteSpace(((PreAuthorizedPaymentReportRow)x).GetLesseeName())));
        }

        #endregion
    }
}
