using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IBeneficiaryRepository : IRepository<Beneficiary>
    {
        Beneficiary BeneficiaryByIdentification(string identification, int idIdentificationType);
        IEnumerable<BeneficiaryList> BeneficiaryListByPolicy(int idPolicy);
    }
}
