using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        IEnumerable<PriorityList> PriorityPagedList(int page, int rows);
    }
}
