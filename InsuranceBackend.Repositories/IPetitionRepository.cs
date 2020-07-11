using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPetitionRepository : IRepository<Petition>
    {
        IEnumerable<PetitionList> PetitionPagedList(int page, int rows, int idCustomer, string state);
    }
}
