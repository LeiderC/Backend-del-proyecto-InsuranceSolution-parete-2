using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyRepository : IRepository<Policy>
    {
        Policy PolicyByIdPolicyOrder(int idPolicyOrder);
        IEnumerable<PolicyList> PolicyPagedList(int page, int rows);
        IEnumerable<PolicyList> PolicyPagedListSearchTerms(string identification, string name, string number, int idcustomer, int iduserpolicyorder, bool isOrder, int page, int rows);
        IEnumerable<PolicyList> PolicyCustomerPagedListSearchTerms(string type, string searchCriteria, int page, int rows);
    }
}
