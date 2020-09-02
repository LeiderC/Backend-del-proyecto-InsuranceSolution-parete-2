using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ICustomerBusinessUnitRepository : IRepository<CustomerBusinessUnit>
    {
        IEnumerable<CustomerBusinessUnitList> CustomerBusinessUnitPagedList(int page, int rows, int idCustomer);

        IEnumerable<CustomerBusinessUnitList> CustomerBusinessUnitListByCustomer(int idCustomer);
    }
}
