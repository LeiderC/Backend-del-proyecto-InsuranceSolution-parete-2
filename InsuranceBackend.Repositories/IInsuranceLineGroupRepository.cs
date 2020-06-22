using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IInsuranceLineGroupRepository : IRepository<InsuranceLineGroup>
    {
        IEnumerable<InsuranceLineGroup> InsuranceLineGroupList(int idInsuranceLine);
    }
}
