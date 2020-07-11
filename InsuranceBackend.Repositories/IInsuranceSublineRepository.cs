using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IInsuranceSublineRepository : IRepository<InsuranceSubline>
    {
        IEnumerable<InsuranceSublineList> InsuranceSublinePagedList(int idInsuranceLine, int page, int rows);
        IEnumerable<InsuranceSubline> InsuranceSublineList(int idInsurance, int idInsuranceLine);
        //InsuranceSubline InsuranceSublineSingle(int id, int idInsurance, int idInsuranceLine);
    }
}
