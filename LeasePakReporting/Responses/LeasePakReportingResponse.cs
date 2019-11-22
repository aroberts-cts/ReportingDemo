using System;
using System.Runtime.Serialization;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.Responses
{
    [DataContract]
    public class LeasePakReportingResponse
    {
        private ILeasePakReport _report { get; }

        [DataMember]
        public LeasePakReportResponse Report { get; set; }
        [DataMember]
        public bool Success { get; set; }

        public LeasePakReportingResponse(ILeasePakReport report, bool success)
        {
            _report = report;
            Success = success && report != null;

            Report = new LeasePakReportResponse(_report);
        }

        public override string ToString()
        {
            var stringRepresentation = $"Report: {Report},{Environment.NewLine}"
                                       + $"Success: {Success}";
            return stringRepresentation;
        }
    }
}