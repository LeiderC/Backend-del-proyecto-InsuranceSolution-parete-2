using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyBeneficiaryRepository : IRepository<PolicyBeneficiary>
    {
        bool DeletePolicyBeneficiaryByPolicy(int idPolicy);
    }
}
