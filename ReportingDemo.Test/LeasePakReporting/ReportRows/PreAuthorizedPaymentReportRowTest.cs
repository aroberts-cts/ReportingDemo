using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportingDemo.LeasePakReporting.ReportRows;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo.Test.LeasePakReporting.ReportRows
{
    [TestClass]
    public class PreAuthorizedPaymentReportRowTest
    {
        #region SetReportRowText

        [TestMethod]
        public void SetReportRowText_HappyPath()
        {
            var lease = "B";
            var lessee = "C";
            var instanceId = "E";
            var payerAccount = "F";
            var invoiceNumber = "G";
            var dueDate = new DateTime(2000,1,1);
            var amount = 1000.00m;

            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText($"A {lease} {lessee} D {instanceId} {payerAccount} {invoiceNumber} {dueDate:MM/dd/yyyy} {amount}", new List<int>() { 1,3,5,7,9,11,13,24 });

            Assert.AreEqual(lease, obj.GetLeaseNumber());
            Assert.AreEqual(lessee, obj.GetLesseeNumber());
            Assert.AreEqual(instanceId, obj.GetInstanceId());
            Assert.AreEqual(payerAccount, obj.GetPayerAccount());
            Assert.AreEqual(invoiceNumber, obj.GetInvoiceNumber());
            Assert.AreEqual(dueDate, obj.GetDueDate());
            Assert.AreEqual(amount, obj.GetAmount());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_EmptyText()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText(string.Empty, new List<int>() { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NullText()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText(null, new List<int>() { 0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_EmptyColumnIndices()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A", new List<int>() { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NullColumnIndices()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_ColumnIndexNotWhitespace()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("ABC", new List<int>() { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetReportRowText_NotEnoughColumnIndices()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A C", new List<int>() { 1 });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedDueDate()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A B C E F G H I", new List<int>() { 1, 3, 5, 7, 9, 11, 13, 15 });
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void SetReportRowText_MalformedAmount()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A B C D E F G 1/1/2000 I", new List<int>() { 1, 3, 5, 7, 9, 11, 13, 22 });
        }

        [TestMethod]
        public void SetReportRowText_HandleCommasInAmount()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            obj.SetReportRowText("A B C D E F G 1/1/2000 1,000.00", new List<int>() { 1, 3, 5, 7, 9, 11, 13, 22 });

            Assert.IsTrue(true);
        }

        #endregion

        #region SetProperties

        [TestMethod]
        public void PopulateResponse_HappyPath()
        {
            var mock = new Mock<ILeasePakReportRowResponse>();

            var obj = new PreAuthorizedPaymentReportRow();
            obj.PopulateResponse(mock.Object);

            mock.Verify(x => x.SetInvoiceNumber(obj.GetInvoiceNumber()));
            mock.Verify(x => x.SetCorporateCostCenter(obj.GetCorporateCostCenter()));
            
            Assert.IsTrue(true);
        }

        #endregion

        #region ToString

        [TestMethod]
        public void ToString_DoesntTriggerExceptionOnEmptyObject()
        {
            var obj = new PreAuthorizedPaymentReportRow();
            var stringRepresentation = obj.ToString();

            Assert.IsTrue(true);
        }

        #endregion
    }
}
