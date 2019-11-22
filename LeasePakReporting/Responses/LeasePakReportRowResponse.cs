using System;
using System.Runtime.Serialization;
using ReportingDemo.LeasePakReporting.ReportRows;

namespace ReportingDemo.LeasePakReporting.Responses
{
    [DataContract]
    public class LeasePakReportRowResponse : ILeasePakReportRowResponse
    {
        private ILeasePakReportRow _row { get; }

        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string LeaseNumber { get; set; }
        [DataMember]
        public string LesseeNumber { get; set; }
        [DataMember]
        public string LesseeName { get; set; }

        [DataMember]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public string CorporateCostCenter { get; set; }

        [DataMember]
        public DateTime DateReceived { get; set; }
        [DataMember]
        public DateTime EffectiveDate { get; set; }
        [DataMember]
        public string LeasePakOperatorInitials { get; set; }
        [DataMember]
        public string PaymentType { get; set; }
        [DataMember]
        public decimal AmountReversed { get; set; }
        [DataMember]
        public DateTime PaymentDueDate { get; set; }

        public LeasePakReportRowResponse(ILeasePakReportRow row)
        {
            _row = row;

            Amount = _row.GetAmount();
            LeaseNumber = _row.GetLeaseNumber();
            LesseeNumber = _row.GetLesseeNumber();
            LesseeName = _row.GetLesseeName();

            row.PopulateResponse(this);
        }

        public override string ToString()
        {
            return _row.ToString();
        }

        public void SetInvoiceNumber(string invoiceNumber)
        {
            InvoiceNumber = invoiceNumber;
        }

        public void SetCorporateCostCenter(string corporateCostCenter)
        {
            CorporateCostCenter = corporateCostCenter;
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
    }
}