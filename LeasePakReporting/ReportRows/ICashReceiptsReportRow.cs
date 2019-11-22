using System;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public interface ICashReceiptsReportRow : ILeasePakReportRow
    {
        void SetDateReceived(DateTime dateReceived);
        void SetEffectiveDate(DateTime effectiveDate);
        void SetLeasePakOperatorInitials(string leasePakOperatorInitials);
        void SetPaymentType(string paymentType);
        void SetAmountReversed(decimal amountReversed);
        void SetPaymentDueDate(DateTime paymentDueDate);
        void SetInvoiceNumber(string invoiceNumber);

        DateTime GetDateReceived();
        DateTime GetEffectiveDate();
        string GetLeasePakOperatorInitials();
        string GetPaymentType();
        decimal GetAmountReversed();
        DateTime GetPaymentDueDate();
        string GetInvoiceNumber();
    }
}
