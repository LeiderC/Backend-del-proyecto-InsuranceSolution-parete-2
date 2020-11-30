using InsuranceBackend.Models;
using System.Collections.Generic;
namespace InsuranceBackend.Repositories
{
    public interface IOnerousRepository : IRepository<Onerous>
    {
        IEnumerable<OnerousList> OnerousPagedList(int page, int rows);
    }
}
