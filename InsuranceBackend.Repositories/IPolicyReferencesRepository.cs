using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyReferencesRepository : IRepository<PolicyReferences>
    {
        bool DeletePolicyReferenciesByPolicy(int idPolicy);
        IEnumerable<PolicyReferencesList> PolicyReferencesListByBolicy(int idPolicy);
    }
}
