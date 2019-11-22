using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingDemo.LeasePakReporting.Responses
{
    public interface ILeasePakReportRowResponse
    {
        void SetInvoiceNumber(string invoiceNumber);
        void SetCorporateCostCenter(string corporateCostCenter);
        void SetDateReceived(DateTime dateReceived);
        void SetEffectiveDate(DateTime effectiveDate);
        void SetLeasePakOperatorInitials(string leasePakOperatorInitials);
        void SetPaymentType(string paymentType);
        void SetAmountReversed(decimal amountReversed);
        void SetPaymentDueDate(DateTime paymentDueDate);
    }
}
