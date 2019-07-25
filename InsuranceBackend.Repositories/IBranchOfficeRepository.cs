using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IBranchOfficeRepository : IRepository<BranchOffice>
    {
        IEnumerable<BranchOfficeList> BranchOfficePagedList(int page, int rows);
    }
}
