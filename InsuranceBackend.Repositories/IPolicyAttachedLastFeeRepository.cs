using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastFeeRepository : IRepository<PolicyAttachedLastFee>
    {
        bool DeleteFeeByPolicy(int idPolicy);
        IEnumerable<PolicyFeeList> PolicyFeeListByPolicy(int idPolicy, bool paid);
        bool DeleteFeeByPolicyFeeNumber(int idPolicy, int feeNumber);
    }
}

