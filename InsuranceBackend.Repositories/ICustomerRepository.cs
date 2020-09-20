using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        IEnumerable<CustomerList> CustomerPagedList(int page, int rows, string searchTerm);
        IEnumerable<CustomerList> CustomerByIdentificationNumber(string identificationNumber, int idSalesman, string type);
        Customer CustomerByIdentificationNumber(string identificationNumber);
        IEnumerable<CustomerList> InsuredListByPolicy(int idPolicy);
        IEnumerable<CustomerList> InsuredAttachedListByPolicy(int idPolicy);
    }
}
