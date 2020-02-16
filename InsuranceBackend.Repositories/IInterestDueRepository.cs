using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IInterestDueRepository : IRepository<InterestDue>
    {
        IEnumerable<InterestDueList> InterestDuePagedList(int page, int rows);
        bool ValidateInterestDue(int idPolicy);
    }
}
