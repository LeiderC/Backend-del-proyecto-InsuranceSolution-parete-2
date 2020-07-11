using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IOccupationRepository : IRepository<Occupation>
    {
        IEnumerable<OccupationList> OccupationPagedList(int page, int rows);
    }
}
