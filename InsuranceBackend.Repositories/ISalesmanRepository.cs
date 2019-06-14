using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ISalesmanRepository : IRepository<Salesman>
    {
        IEnumerable<SalesmanList> SalesmanPagedList(int page, int rows);
    }
}
