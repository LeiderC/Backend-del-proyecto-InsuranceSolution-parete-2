using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IManagementTypeRepository : IRepository<ManagementType>
    {
        IEnumerable<ManagementTypeList> ManagementTypePagedList(int page, int rows);
    }
}
