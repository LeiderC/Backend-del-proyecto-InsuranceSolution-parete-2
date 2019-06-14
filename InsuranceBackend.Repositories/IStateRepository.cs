using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IStateRepository : IRepository<State>
    {
        IEnumerable<StateList> StatePagedList(int page, int rows);
    }
}
