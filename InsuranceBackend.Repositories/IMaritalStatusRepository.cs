using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IMaritalStatusRepository : IRepository<MaritalStatus>
    {
        IEnumerable<MaritalStatusList> MaritalStatusPagedList(int page, int rows);
    }
}
