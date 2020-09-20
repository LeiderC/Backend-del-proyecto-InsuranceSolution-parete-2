using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastBeneficiaryRepository : IRepository<PolicyAttachedLastBeneficiary>
    {
        bool DeletePolicyBeneficiaryByPolicy(int idPolicy);
    }
}
