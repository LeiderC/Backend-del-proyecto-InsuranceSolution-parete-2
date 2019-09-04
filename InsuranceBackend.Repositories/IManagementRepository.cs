using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IManagementRepository : IRepository<Management>
    {
        IEnumerable<ManagementList> ManagementPagedList(int idCustomer, int page, int rows);
    }
}
