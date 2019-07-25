using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyRepository : IRepository<Policy>
    {
        IEnumerable<PolicyList> PolicyPagedList(int page, int rows);
    }
}
