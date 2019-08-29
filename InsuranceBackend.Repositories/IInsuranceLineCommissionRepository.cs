using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IInsuranceLineCommissionRepository : IRepository<InsuranceLineCommission>
    {
        IEnumerable<InsuranceLineCommissionList> InsuranceLineCommissionPagedList(int idInsuranceLine, int page, int rows);
        InsuranceLineCommission InsuranceLineCommissionSingle(int idInsurance, int idInsuranceLine);
    }
}
