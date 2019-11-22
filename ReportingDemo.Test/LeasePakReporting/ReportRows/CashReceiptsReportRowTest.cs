using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportingDemo.LeasePakReporting.ReportRows;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo.Test.LeasePakReporting.ReportRows
{
    [TestClass]
    public class CashReceiptsReportRowTest
    {
        #region SetReportRowText

        [TestMethod]
        public void SetReportRowText_HappyPath()
        {
            var lease = "A";
            var dateReceived = new DateTime(2000,1,1);
            var effectiveDate = new DateTime(2001,1,1);
            var leasePakOperatorInitials = "F";
            var paymentType = "G";
            var amount = 1000.00m;
            var amountReversed = 2000.00m;
            var paymentDueDate = new DateTime(2002,1,1);
            var invoiceNumber = "K";

            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText($"{lease} B {dateReceived:MM/dd/yyyy} D {effectiveDate:MM/dd/yyyy} {leasePakOperatorInitials} {paymentType} {amount} {amountReversed} {paymentDueDate:MM/dd/yyyy} {invoiceNumber} L", new List<int>() { 1, 3, 14, 16, 27, 29, 31, 39, 47, 58, 60 });

            Assert.AreEqual(lease, obj.GetLeaseNumber());
            Assert.AreEqual(dateReceived, obj.GetDateReceived());
            Assert.AreEqual(effectiveDate, obj.GetEffectiveDate());
            Assert.AreEqual(leasePakOperatorInitials, obj.GetLeasePakOperatorInitials());
            Assert.AreEqual(paymentType, obj.GetPaymentType());
            Assert.AreEqual(amount, obj.GetAmount());
            Assert.AreEqual(amountReversed, obj.GetAmountReversed());
            Assert.AreEqual(paymentDueDate, obj.GetPaymentDueDate());
            Assert.AreEqual(invoiceNumber, obj.GetInvoiceNumber());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_EmptyText()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText(string.Empty, new List<int>() { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NullText()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText(null, new List<int>() { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_EmptyColumnIndices()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A", new List<int>() { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NullColumnIndices()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_ColumnIndexNotWhitespace()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("ABC", new List<int>() { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NotEnoughColumnIndices()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A C", new List<int>() { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedDateReceived()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B C D E F G H I J K L", new List<int>() { 1,3,5,7,9,11,13,15,17,19,21 });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedEffectiveDate()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D E F G H I J K L", new List<int>() { 1,3,12,14,16,18,20,22,24,26,28 });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedAmount()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D 1/1/2000 F G H I J K L", new List<int>() { 1,3,12,14,23,25,27,29,31,33,35, });
        }

        [TestMethod]
        public void SetReportRowText_HandleCommasInAmount()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D 1/1/2000 F G 1,000.00 0.00 1/1/2000 K L", new List<int>() { 1,3,12,14,23,25,27,36,41,50,52 });

            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedAmountReversed()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D 1/1/2000 F G 1000.00 I J K L", new List<int>() { 1,3,12,14,23,25,27,35,37,39,41 });
        }

        [TestMethod]
        public void SetReportRowText_HandleCommasInAmountReversed()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D 1/1/2000 F G 1000.00 1,000.00 1/1/2000 K L", new List<int>() { 1,3,12,14,23,25,27,35,44,53,55 });

            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedPaymentDueDate()
        {
            var obj = new CashReceiptsReportRow();
            obj.SetReportRowText("A B 1/1/2000 D 1/1/2000 F G 1000.00 1000.00 I J K :", new List<int>() { 1,3,12,14,23,25,27,35,43,45,47 });
        }

        #endregion

        #region PopulateReponse

        [TestMethod]
        public void PopulateResponse_HappyPath()
        {
            var mock = new Mock<ILeasePakReportRowResponse>();

            var obj = new CashReceiptsReportRow();
            obj.PopulateResponse(mock.Object);

            mock.Verify(x => x.SetDateReceived(obj.GetDateReceived()));
            mock.Verify(x => x.SetPaymentDueDate(obj.GetPaymentDueDate()));
            mock.Verify(x => x.SetAmountReversed(obj.GetAmountReversed()));
            mock.Verify(x => x.SetEffectiveDate(obj.GetEffectiveDate()));
            mock.Verify(x => x.SetLeasePakOperatorInitials(obj.GetLeasePakOperatorInitials()));
            mock.Verify(x => x.SetPaymentType(obj.GetPaymentType()));

            Assert.IsTrue(true);
        }

        #endregion

        #region ToString

        [TestMethod]
        public void ToString_DoesntTriggerExceptionOnEmptyObject()
        {
            var obj = new CashReceiptsReportRow();
            var stringRepresentation = obj.ToString();

            Assert.IsTrue(true);
        }

        #endregion
    }
}
