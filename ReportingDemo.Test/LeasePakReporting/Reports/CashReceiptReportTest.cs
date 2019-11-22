using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportingDemo.LeasePakReporting.Reports;
using ReportingDemo.Repositories.Calendar;
using ReportingDemo.Repositories.File;

namespace ReportingDemo.Test.LeasePakReporting.Reports
{
    [TestClass]
    public class CashReceiptReportTest
    {
        #region GetFilePath

        [TestMethod]
        public void GetFilePath_HappyPath()
        {
            var mock = new Mock<ICalendarRepository>();
            mock.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var portfolio = "A";
            var obj = new CashReceiptsReport(null, null, mock.Object);
            obj.SetPortfolio(portfolio);
            obj.SetReportDate(DateTime.Today);

            var filePath = obj.GetFilePath();

            // Have to get two business days ago
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            Assert.AreEqual($@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{DateTime.Today:MMddyy}\p{portfolio}_{Constants.LeasePakReporting.CashReceiptsReportFileName}", filePath);
        }

        [TestMethod]
        public void GetFilePath_UseCachedPath()
        {
            var mock = new Mock<ICalendarRepository>();
            mock.Setup(x => x.GetPreviousBusinessDay(It.IsAny<DateTime>())).Returns(DateTime.Today);

            var portfolio = "A";
            var obj = new CashReceiptsReport(null, null, mock.Object);
            obj.SetPortfolio(portfolio);
            obj.SetReportDate(DateTime.Today);

            obj.GetFilePath();

            // Have to get two business days ago
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            var filePath = obj.GetFilePath();

            // Make sure it didn't call out to the function again
            mock.Verify(x => x.GetPreviousBusinessDay(DateTime.Today), Times.Exactly(2));

            Assert.AreEqual($@"{RuntimeSettings.LeasePakReporting.ReportingFolderFilepath}\{DateTime.Today:MMddyy}\p{portfolio}_{Constants.LeasePakReporting.CashReceiptsReportFileName}", filePath);
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

            var obj = new CashReceiptsReport(null, mockFile.Object, mockCalendar.Object);
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

            var obj = new CashReceiptsReport(null, mockFile.Object, mockCalendar.Object);
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

            var obj = new CashReceiptsReport(null, mockFile.Object, mockCalendar.Object);
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
            var lease = "A";
            var dateReceived = new DateTime(2000, 1, 1);
            var effectiveDate = new DateTime(2001, 1, 1);
            var leasePakOperatorInitials = "F";
            var paymentType = "G";
            var amount = 1000.00m;
            var amountReversed = 2000.00m;
            var paymentDueDate = new DateTime(2002, 1, 1);
            var invoiceNumber = "K";

            var reportLine = $"{lease} B {dateReceived:MM/dd/yyyy} D {effectiveDate:MM/dd/yyyy} {leasePakOperatorInitials} {paymentType} {amount} {amountReversed} {paymentDueDate:MM/dd/yyyy} {invoiceNumber} L";
            var parsedText = new List<string>() {reportLine, reportLine, reportLine};

            var obj = new CashReceiptsReport(null, null, null);
            obj.SetColumnSplitIndices(new List<int>() { 1, 3, 14, 16, 27, 29, 31, 39, 47, 58, 60 });
            obj.AddDataRows(parsedText);

            var dataRows = obj.GetDataRows();

            Assert.IsTrue(dataRows.Any());
            Assert.AreEqual(parsedText.Count, dataRows.Count);
            Assert.IsTrue(dataRows.All(x => x != null));
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

        #endregion
    }
}
