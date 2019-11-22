using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ReportingDemo
{
    [DataContract]
    public static class Constants
    {
        public static class Common
        {
            public static class ConnectionStrings
            {
                public const string LeasePakConnectionStringName = "LeasePakConnectionString";
                public const string LeasePakSybaseConnectionStringName = "LeasePakSybaseConnectionString";
                public const string MiTransferConnectionStringName = "MiDataExchangeConnectionString";
            }
        }

        [DataContract]
        public static class LeasePakReporting
        {
            #region ReportEnums
            [DataContract]
            public enum Report
            {
                [EnumMember]
                PreAuthorizedPayments,
                [EnumMember]
                CashReceipts
            }

            #endregion

            #region AppSettings

            public static class AppSettings
            {
                public const string ReportingFolderFilepathAppSettingName = "LeasePakReportingFolderFilepath";
            }

            #endregion

            #region ReportNames

            public const string CashReceiptsReportFileName = "lpr0411.rpt";
            public const string PreAuthorizedPaymentsReportFileName = "lpu0304a.rpt";

            #endregion
        }
    }
}