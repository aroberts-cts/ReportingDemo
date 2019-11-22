using System;
using System.Collections.Generic;
using System.Linq;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public class CashReceiptsReportRow : AbstractLeasePakReportRow, ICashReceiptsReportRow
    {
        private const int NumberOfColumnSplitsRequired = 11;

        private DateTime DateReceived { get; set; }
        private DateTime EffectiveDate { get; set; }
        private string LeasePakOperatorInitials { get; set; }
        private string PaymentType { get; set; }
        private decimal AmountReversed { get; set; }
        private DateTime PaymentDueDate { get; set; }
        private string InvoiceNumber { get; set; }

        public void SetReportRowText(string rowText, IList<int> columnSplitIndices)
        {
            if (string.IsNullOrWhiteSpace(rowText)) throw new ArgumentException("rowText cannot be null.");
            if (columnSplitIndices == null) throw new ArgumentException("columnSplitIndices cannot be null");
            if (!columnSplitIndices.Any()) throw new ArgumentException("columnSplitIndices cannot be empty.");
            if (!columnSplitIndices.Select(i => string.IsNullOrWhiteSpace(rowText[i].ToString())).Any()) throw new ArgumentException("columnSplitIndices can only be assigned at whitespace in the rowText.");
            if (columnSplitIndices.Count != NumberOfColumnSplitsRequired) throw new ArgumentException($"This report requires exactly {NumberOfColumnSplitsRequired} columnSplitIndices to function properly.");

            ReportRowTextRaw = rowText;

            LeaseNumber = ReportRowTextRaw
                .Substring(0, columnSplitIndices[0])
                .Trim();

            // Index 0 is the Lessee Name but it gets trimmed in the file

            DateReceived = Convert.ToDateTime(ReportRowTextRaw
                .Substring(columnSplitIndices[1], columnSplitIndices[2] - columnSplitIndices[1])
                .Trim()
            );

            // Index 2 is the time, not relevant

            EffectiveDate = Convert.ToDateTime(ReportRowTextRaw
                .Substring(columnSplitIndices[3], columnSplitIndices[4] - columnSplitIndices[3])
                .Trim()
            );
            LeasePakOperatorInitials = ReportRowTextRaw
                .Substring(columnSplitIndices[4], columnSplitIndices[5] - columnSplitIndices[4])
                .Trim();
            PaymentType = ReportRowTextRaw
                .Substring(columnSplitIndices[5], columnSplitIndices[6] - columnSplitIndices[5])
                .Trim();
            Amount = Convert.ToDecimal(ReportRowTextRaw
                .Substring(columnSplitIndices[6], columnSplitIndices[7] - columnSplitIndices[6])
                .Trim()
                .Replace(",", string.Empty)
            );
            AmountReversed = Convert.ToDecimal(ReportRowTextRaw
                .Substring(columnSplitIndices[7], columnSplitIndices[8] - columnSplitIndices[7])
                .Trim()
                .Replace(",", string.Empty)
            );
            PaymentDueDate = Convert.ToDateTime(ReportRowTextRaw
                .Substring(columnSplitIndices[8], columnSplitIndices[9] - columnSplitIndices[8])
                .Trim()
            );
            InvoiceNumber = ReportRowTextRaw
                .Substring(columnSplitIndices[9], columnSplitIndices[10] - columnSplitIndices[9])
                .Trim();
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

        public void SetLesseeName(string lesseeName)
        {
            LesseeName = lesseeName;
        }

        public void PopulateResponse(ILeasePakReportRowResponse response)
        {
            response.SetDateReceived(GetDateReceived());
            response.SetEffectiveDate(GetEffectiveDate());
            response.SetLeasePakOperatorInitials(GetLeasePakOperatorInitials());
            response.SetPaymentType(GetPaymentType());
            response.SetAmountReversed(GetAmountReversed());
            response.SetPaymentDueDate(GetPaymentDueDate());
        }

        public void SetDateReceived(DateTime dateReceived)
        {
            DateReceived = dateReceived;
        }

        public void SetEffectiveDate(DateTime effectiveDate)
        {
            EffectiveDate = effectiveDate;
        }

        public void SetLeasePakOperatorInitials(string leasePakOperatorInitials)
        {
            LeasePakOperatorInitials = leasePakOperatorInitials;
        }

        public void SetPaymentType(string paymentType)
        {
            PaymentType = paymentType;
        }

        public void SetAmountReversed(decimal amountReversed)
        {
            AmountReversed = amountReversed;
        }

        public void SetPaymentDueDate(DateTime paymentDueDate)
        {
            PaymentDueDate = paymentDueDate;
        }

        public void SetInvoiceNumber(string invoiceNumber)
        {
            InvoiceNumber = invoiceNumber;
        }

        public DateTime GetDateReceived()
        {
            return DateReceived;
        }

        public DateTime GetEffectiveDate()
        {
            return EffectiveDate;
        }

        public string GetLeasePakOperatorInitials()
        {
            return LeasePakOperatorInitials;
        }

        public string GetPaymentType()
        {
            return PaymentType;
        }

        public decimal GetAmountReversed()
        {
            return AmountReversed;
        }

        public DateTime GetPaymentDueDate()
        {
            return PaymentDueDate;
        }

        public string GetInvoiceNumber()
        {
            return InvoiceNumber;
        }

        public override string ToString()
        {
            var baseString = base.ToString();

            // Remove the closing brace
            baseString = baseString.Remove(baseString.Length - 4);

            baseString += $",{Environment.NewLine}"
                          + $"  InvoiceNumber: {GetInvoiceNumber()}{Environment.NewLine}"
                          + $"  DateReceived: {GetDateReceived():MM/dd/yyyy},{Environment.NewLine}"
                          + $"  EffectiveDate: {GetEffectiveDate():MM/dd/yyyy},{Environment.NewLine}"
                          + $"  LeasePakOperatorInitials: {GetLeasePakOperatorInitials()}{Environment.NewLine}"
                          + $"  PaymentType: {GetPaymentType()}{Environment.NewLine}"
                          + $"  AmountReversed: {GetAmountReversed()}{Environment.NewLine}"
                          + $"  PaymentDueDate: {GetPaymentDueDate()}{Environment.NewLine}";

            // Add the closing brace back
            baseString += "}";

            return baseString;
        }
    }
}