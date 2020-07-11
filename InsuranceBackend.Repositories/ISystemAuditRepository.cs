using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ISystemAuditRepository : IRepository<SystemAudit>
    {
        IEnumerable<SystemAuditList> SystemAuditPagedList(int page, int rows);
    }
}
