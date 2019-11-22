using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportingDemo.LeasePakReporting.ReportParsers;

namespace ReportingDemo.Test
{
    [TestClass]
    public class ReportingDemoTest
    {
        #region GetParserFromReportEnum

        [TestMethod]
        public void GetParserFromReportEnum_ValidEnum()
        {
            var output =
                ReportingDemo.GetParserFromReportEnum(Constants.LeasePakReporting.Report.CashReceipts);

            Assert.IsNotNull(output);
        }

        #endregion
}
}
