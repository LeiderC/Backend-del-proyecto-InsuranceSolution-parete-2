using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPrefixRepository : IRepository<Prefix>
    {
        IEnumerable<PrefixList> PrefixPagedList(int page, int rows);
    }
}
