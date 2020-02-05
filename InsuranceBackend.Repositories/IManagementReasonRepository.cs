using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IManagementReasonRepository : IRepository<ManagementReason>
    {
        IEnumerable<ManagementReasonList> ManagementReasonPagedList(int page, int rows);
    }
}
