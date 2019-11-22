using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ReportingDemo.LeasePakReporting.Responses;

namespace ReportingDemo
{
    [ServiceContract]
    public interface IReportingDemo
    {
        [OperationContract]
        LeasePakReportingResponse ParseLeasePakReport(DateTime businessDay, string portfolio, Constants.LeasePakReporting.Report report);
    }
}
