using MiFramework.Common.Utilities;
using MiFramework.Data.LeasePak;

namespace ReportingDemo.Repositories.Lessee
{
    public class LesseeRepository : ILesseeRepository
    {
        public LesseeRepository()
        {
            LeasePak.Init(RuntimeSettings.ConnectionStrings.LeasePakConnectionString);
        }

        public string GetLesseeNameFromLesseeNumber(string lesseeNumber)
        {
            return LeasePak.ral.SelectByLesseeNumber(AccountingUtilities.PadLessee(lesseeNumber))?.nam_long_s.Trim();
        }
    }
}