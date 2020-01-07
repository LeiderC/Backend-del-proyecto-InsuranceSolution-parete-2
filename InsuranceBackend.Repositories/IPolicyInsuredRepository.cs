using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyInsuredRepository : IRepository<PolicyInsured>
    {
        bool DeletePolicyInsuredByPolicy(int idPolicy);
    }
}
