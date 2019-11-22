using System;
using System.Collections.Generic;
using System.Linq;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public class PreAuthorizedPaymentReportRow : AbstractLeasePakReportRow, IPreAuthorizedPaymentReportRow
    {
        private const int NumberOfColumnSplitsRequired = 8;

        private string InvoiceNumber { get; set; }
        private string CorporateCostCenter { get; set; }
        private string InstanceId { get; set; }
        private string PayerAccount { get; set; }
        private DateTime DueDate { get; set; }

        public void SetReportRowText(string reportRowText, IList<int> columnSplitIndices)
        {
            if (string.IsNullOrWhiteSpace(reportRowText)) throw new ArgumentException("rowText cannot be null.");
            if (columnSplitIndices == null) throw new ArgumentException("columnSplitIndices cannot be null");
            if (!columnSplitIndices.Any()) throw new ArgumentException("columnSplitIndices cannot be empty.");
            if (!columnSplitIndices.Select(i => string.IsNullOrWhiteSpace(reportRowText[i].ToString())).Any()) throw new ArgumentException("columnSplitIndices can only be assigned at whitespace in the rowText.");
            if (columnSplitIndices.Count != NumberOfColumnSplitsRequired) throw new ArgumentException($"This report requires exactly {NumberOfColumnSplitsRequired} columnSplitIndices to function properly.");

            ReportRowTextRaw = reportRowText;

            // Index 0 is redundant portfolio information, we already needed it to get the file name

            LeaseNumber = ReportRowTextRaw
                .Substring(columnSplitIndices[0], columnSplitIndices[1] - columnSplitIndices[0])
                .Trim();
            LesseeNumber = ReportRowTextRaw
                .Substring(columnSplitIndices[1], columnSplitIndices[2] - columnSplitIndices[1])
                .Trim();
            
            // Index 3 is the Lessee Name but it gets trimmed in the file so we look this up elsewhere

            InstanceId = ReportRowTextRaw
                .Substring(columnSplitIndices[3], columnSplitIndices[4] - columnSplitIndices[3])
                .Trim();
            PayerAccount = ReportRowTextRaw
                .Substring(columnSplitIndices[4], columnSplitIndices[5] - columnSplitIndices[4])
                .Trim();
            InvoiceNumber = ReportRowTextRaw
                .Substring(columnSplitIndices[5], columnSplitIndices[6] - columnSplitIndices[5])
                .Trim();
            DueDate = Convert.ToDateTime(ReportRowTextRaw
                .Substring(columnSplitIndices[6], columnSplitIndices[7] - columnSplitIndices[6])
                .Trim()
            );
            Amount = Convert.ToDecimal(ReportRowTextRaw
                .Substring(columnSplitIndices[7], ReportRowTextRaw.Length-1 - columnSplitIndices[7])
                .Trim()
                .Replace(",", string.Empty)
            );
        }

        public string GetLeaseNumber()
        {
            return LeaseNumber;
        }

        public decimal GetAmount()
        {
            return Amount;
        }

        public string GetLesseeNumber()
        {
            return LesseeNumber;
        }

        public string GetLesseeName()
        {
            return LesseeName;
        }

        public string GetInvoiceNumber()
        {
            return InvoiceNumber;
        }

        public string GetCorporateCostCenter()
        {
            return CorporateCostCenter;
        }

        public string GetInstanceId()
        {
            return InstanceId;
        }

        public string GetPayerAccount()
        {
            return PayerAccount;
        }

        public DateTime GetDueDate()
        {
            return DueDate;
        }

        public void SetInvoiceNumber(string invoiceNumber)
        {
            InvoiceNumber = invoiceNumber;
        }

        public void SetLesseeName(string lesseeName)
        {
            LesseeName = lesseeName;
        }

        public void PopulateResponse(ILeasePakReportRowResponse response)
        {
            response.SetInvoiceNumber(GetInvoiceNumber());
            response.SetCorporateCostCenter(GetCorporateCostCenter());
        }

        public void SetCorporateCostCenter(string corporateCostCenter)
        {
            CorporateCostCenter = corporateCostCenter;
        }

        public void SetInstanceId(string instanceId)
        {
            InstanceId = instanceId;
        }

        public void SetPayerAccount(string payerAccount)
        {
            PayerAccount = payerAccount;
        }

        public void SetDueDate(DateTime dueDate)
        {
            DueDate = dueDate;
        }

        public override string ToString()
        {
            var baseString = base.ToString();

            // Remove the closing brace
            baseString = baseString.Remove(baseString.Length - 4);

            baseString += $",{Environment.NewLine}"
                      + $"  InvoiceNumber: {GetInvoiceNumber()}{Environment.NewLine}"
                      + $"  InstanceId: {GetInstanceId()},{Environment.NewLine}"
                      + $"  PayerAccount: {GetPayerAccount()},{Environment.NewLine}"
                      + $"  DueDate: {GetDueDate():MM/dd/yyyy},{Environment.NewLine}"
                      + $"  CorporateCostCenter: {GetCorporateCostCenter()}{Environment.NewLine}";

            // Add the closing brace back
            baseString += "}";

            return baseString;
        }
    }
}