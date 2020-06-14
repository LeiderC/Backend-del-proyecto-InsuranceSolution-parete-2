using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IExternalSalesmanParamRepository : IRepository<ExternalSalesmanParam>
    {
        IEnumerable<ExternalSalesmanParamList> ExternalSalesmanParamPagedList(int page, int rows);
    }
}
