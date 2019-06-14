using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ICustomerTypeRepository: IRepository<CustomerType>
    {
        IEnumerable<CustomerTypeList> CustomerTypePagedList(int page, int rows);
    }
}
