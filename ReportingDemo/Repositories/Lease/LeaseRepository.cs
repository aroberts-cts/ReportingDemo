using System.Linq;
using MiFramework.Common.Utilities;
using MiFramework.Data.LeasePak;
using MiFramework.Data.LeasePak.Domain;
using MiFramework.Data.MiDataExchange;

namespace ReportingDemo.Repositories.Lease
{
    public class LeaseRepository : ILeaseRepository
    {
        public LeaseRepository()
        {
            LeasePak.Init(RuntimeSettings.ConnectionStrings.LeasePakConnectionString);
            MiDataExchange.Init(RuntimeSettings.ConnectionStrings.MiTransferConnectionString);
        }

        public rls GetLeaseByLeaseNumber(string leaseNumber)
        {
            return LeasePak.rls.SelectByLeaseNumber(AccountingUtilities.PadLease(leaseNumber));
        }

        public string GetCorporateCostCenterByLeaseNumber(string leaseNumber)
        {
            var lease = GetLeaseByLeaseNumber(leaseNumber);
            if (lease == null) return null;

            var costCenter = MiDataExchange.LPGLMOfficeMap.SelectMapsByLPOfficeNumber(lease.off_s)
                .FirstOrDefault(m => m.Portfolio.Trim().Equals(lease.por_s.Trim()))?.PSOfficeNumber;
            return costCenter;
        }
    }
}