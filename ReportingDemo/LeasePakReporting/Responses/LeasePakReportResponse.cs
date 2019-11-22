using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ReportingDemo.LeasePakReporting.Reports;

namespace ReportingDemo.LeasePakReporting.Responses
{
    [DataContract]
    public class LeasePakReportResponse
    {
        private ILeasePakReport _report { get; }

        [DataMember] 
        public IList<LeasePakReportRowResponse> Rows { get; }
        [DataMember]
        public byte[] Bytes { get; set; }

        public LeasePakReportResponse(ILeasePakReport report)
        {
            _report = report;

            Rows = _report?.GetDataRows()
                .Select(r => new LeasePakReportRowResponse(r))
                .ToList();
            Bytes = _report?.GetBytes();
        }

        public override string ToString()
        {
            var stringRepresentation = $"Rows: {Rows}";
            return stringRepresentation;
        }
    }
}