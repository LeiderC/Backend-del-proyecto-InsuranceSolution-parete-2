using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyFeeFinancialRepository : IRepository<PolicyFeeFinancial>
    {
        bool DeleteFeeByPolicy(int idPolicy);
        IEnumerable<PolicyFeeFinancialList> PolicyFeeFinancialListByPolicy(int idPolicy, bool paid);
    }
}
