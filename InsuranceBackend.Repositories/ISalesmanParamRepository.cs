using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ISalesmanParamRepository : IRepository<SalesmanParam>
    {
        IEnumerable<SalesmanParamList> SalesmanParamPagedList(int page, int rows, int idSalesman);
    }
}
