using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyFeeRepository : IRepository<PolicyFee>
    {
        bool DeleteFeeByPolicy(int idPolicy);

        IEnumerable<PolicyFeeList> PolicyFeeListByPolicy(int idPolicy, bool paid);
    }
}
