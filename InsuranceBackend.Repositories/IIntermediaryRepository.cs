using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IIntermediaryRepository : IRepository<Intermediary>
    {
        IEnumerable<IntermediaryList> IntermediaryPagedList(int page, int rows);
    }
}
