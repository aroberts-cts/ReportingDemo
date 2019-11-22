using MiFramework.Data.LeasePak.Domain;

namespace ReportingDemo.Repositories.Lease
{
    public interface ILeaseRepository
    {
        rls GetLeaseByLeaseNumber(string leaseNumber);
        string GetCorporateCostCenterByLeaseNumber(string leaseNumber);
    }
}
