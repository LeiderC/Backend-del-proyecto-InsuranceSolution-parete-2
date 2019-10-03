using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IManagementRepository : IRepository<Management>
    {
        IEnumerable<ManagementList> ManagementPagedList(int page, int rows);
    }
}
