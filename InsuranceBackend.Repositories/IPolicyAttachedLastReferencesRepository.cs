using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastReferencesRepository : IRepository<PolicyAttachedLastReferences>
    {
        bool DeletePolicyReferenciesByPolicy(int idPolicy);
        IEnumerable<PolicyReferencesList> PolicyReferencesListByBolicy(int idPolicy);
    }
}
