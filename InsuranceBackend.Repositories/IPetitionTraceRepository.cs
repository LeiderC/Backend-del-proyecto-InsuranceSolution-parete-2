using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPetitionTraceRepository : IRepository<PetitionTrace>
    {
        IEnumerable<PetitionTraceList> PetitionTracePagedList(int page, int rows, int idPetition);
    }
}
