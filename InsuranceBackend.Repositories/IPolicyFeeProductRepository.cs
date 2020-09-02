using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyFeeProductRepository : IRepository<PolicyFeeProduct>
    {
        bool DeleteFeeProductByPolicy(int idPolicy);
        IEnumerable<PolicyFeeProductList> PolicyFeeProductListByPolicy(int idPolicy, bool paid);
        bool DeleteFeeByPolicyFeeNumber(int idPolicy, int feeNumber);
    }
}

