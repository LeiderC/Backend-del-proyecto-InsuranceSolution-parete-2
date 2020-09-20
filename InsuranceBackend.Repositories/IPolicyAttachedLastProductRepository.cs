using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastProductRepository : IRepository<PolicyAttachedLastProduct>
    {
        bool DeletePolicyProductByPolicy(int idPolicy);
        IEnumerable<PolicyProductList> PolicyProductListByPolicy(int idPolicy);
    }
}
