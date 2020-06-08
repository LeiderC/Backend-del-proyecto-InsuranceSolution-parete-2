using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IWaytoPayRepository : IRepository<WaytoPay>
    {
        IEnumerable<WaytoPay> GetWaytoPaysByPaymentType(string idPaymentType);
    }
}
