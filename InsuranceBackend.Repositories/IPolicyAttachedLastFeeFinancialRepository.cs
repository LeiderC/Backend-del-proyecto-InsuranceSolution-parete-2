using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastFeeFinancialRepository : IRepository<PolicyAttachedLastFeeFinancial>
    {
        bool DeleteFeeByPolicy(int idPolicy);
        IEnumerable<PolicyFeeFinancialList> PolicyFeeFinancialListByPolicy(int idPolicy, bool paid);
    }
}
