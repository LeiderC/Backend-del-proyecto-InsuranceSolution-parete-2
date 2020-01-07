using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyProductRepository : IRepository<PolicyProduct>
    {
        bool DeletePolicyProductByPolicy(int idPolicy);
    }
}
