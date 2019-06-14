using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IEPSRepository : IRepository<EPS>
    {
        IEnumerable<EPSList> EPSPagedList(int page, int rows);
    }
}
 
