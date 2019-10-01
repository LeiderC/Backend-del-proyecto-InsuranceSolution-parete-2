using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IFinancialRepository : IRepository<Financial>
    {
        IEnumerable<FinancialList> FinancialPagedList(int page, int rows);
    }
}
