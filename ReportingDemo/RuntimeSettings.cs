using System.Configuration;

namespace ReportingDemo
{
    public static class RuntimeSettings
    {
        public static class ConnectionStrings
        {
            public static string LeasePakConnectionString => ConfigurationManager
                .ConnectionStrings[Constants.Common.ConnectionStrings.LeasePakConnectionStringName].ConnectionString;

            public static string LeasePakSybaseConnectionString => ConfigurationManager
                .ConnectionStrings[Constants.Common.ConnectionStrings.LeasePakSybaseConnectionStringName]
                .ConnectionString;
            public static string MiTransferConnectionString =>
                ConfigurationManager.ConnectionStrings[
                    Constants.Common.ConnectionStrings.MiTransferConnectionStringName].ConnectionString;
        }

        public static class LeasePakReporting
        {
            public static string ReportingFolderFilepath =>
                ConfigurationManager.AppSettings.Get(Constants.LeasePakReporting.AppSettings.ReportingFolderFilepathAppSettingName);
        }
    }
}