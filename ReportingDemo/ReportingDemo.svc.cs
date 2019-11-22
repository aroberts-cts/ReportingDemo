using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ReportingDemo.LeasePakReporting.Factories;
using ReportingDemo.LeasePakReporting.ReportParsers;
using ReportingDemo.LeasePakReporting.Responses;
using ReportingDemo.Repositories.Log;

namespace ReportingDemo
{
    public class ReportingDemo : IReportingDemo
    {
        public LeasePakReportingResponse ParseLeasePakReport(DateTime businessDay, string portfolio, Constants.LeasePakReporting.Report report)
        {
            var logger = LogRepository.GetLogger<ReportingDemo>();
            logger.Info($"Initiating request.  Business Day: {businessDay:MM/dd/yyyy}, Portfolio: {portfolio}, Report: {report}");

            var reportParser = GetParserFromReportEnum(report);

            logger.Info($"Initiating report parsing.");
            var parsedReport = reportParser.Parse(businessDay, portfolio);
            var success = parsedReport != null;
            logger.Info($"Report parsing complete.  Success: {success}");

            var response = new LeasePakReportingResponse(parsedReport, success);
            logger.Info($"Request complete.  Response: {response}");

            return response;
        }

        // Have to switch from enum to concrete type, since you can't pass a type parameter through a WCF call. Ideally this wouldn't exist.
        public static ILeasePakReportParser GetParserFromReportEnum(Constants.LeasePakReporting.Report report)
        {
            switch (report)
            {
                case Constants.LeasePakReporting.Report.PreAuthorizedPayments:
                    return LeasePakReportParserFactory.Create<PreAuthorizedPaymentReportParser>();
                case Constants.LeasePakReporting.Report.CashReceipts:
                    return LeasePakReportParserFactory.Create<CashReceiptsReportParser>();
            }

            throw new NotImplementedException($"Report type {report} has not yet had a report parser implemented for it.");
        }
    }
}
