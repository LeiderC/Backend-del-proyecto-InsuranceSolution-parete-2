using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IExternalSalesmanRepository : IRepository<ExternalSalesman>
    {
        IEnumerable<ExternalSalesmanList> ExternalSalesmanPagedList(int page, int rows);
    }
}
