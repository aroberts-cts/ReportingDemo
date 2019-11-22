using System;

namespace ReportingDemo.LeasePakReporting.ReportRows
{
    public abstract class AbstractLeasePakReportRow
    {
        public string ReportRowTextRaw { get; protected set; }

        protected string LeaseNumber { get; set; }
        protected string LesseeNumber { get; set; }
        protected string LesseeName { get; set; }
        protected decimal Amount { get; set; }

        public override string ToString()
        {
            var stringRepresentation =
                $"{{{Environment.NewLine}"
                + $"  Amount: {Amount:C2},{Environment.NewLine}"
                + $"  LeaseNumber: {LeaseNumber},{Environment.NewLine}"
                + $"  LesseeNumber: {LesseeNumber},{Environment.NewLine}"
                + $"  LesseeName: {LesseeName},{Environment.NewLine}"
                + "}";
            return stringRepresentation;
        }
    }
}