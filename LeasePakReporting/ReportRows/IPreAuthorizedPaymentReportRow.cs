using System;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public interface IPreAuthorizedPaymentReportRow : ILeasePakReportRow
    {
        string GetInvoiceNumber();
        string GetCorporateCostCenter();
        string GetInstanceId();
        string GetPayerAccount();
        DateTime GetDueDate();

        void SetInvoiceNumber(string invoiceNumber);
        void SetCorporateCostCenter(string corporateCostCenter);
        void SetInstanceId(string instanceId);
        void SetPayerAccount(string payerAccount);
        void SetDueDate(DateTime dueDate);
    }
}
