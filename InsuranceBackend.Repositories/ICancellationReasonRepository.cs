using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ICancellationReasonRepository : IRepository<CancellationReason>
    {
        IEnumerable<CancellationReasonList> CancellationReasonPagedList(int page, int rows);
    }
}
