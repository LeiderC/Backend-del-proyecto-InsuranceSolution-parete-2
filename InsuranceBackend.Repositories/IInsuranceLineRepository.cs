using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IInsuranceLineRepository : IRepository<InsuranceLine>
    {
        IEnumerable<InsuranceLineList> InsuranceLinePagedList(int page, int rows);
    }
}
