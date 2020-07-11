using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IGenderRepository : IRepository<Gender>
    {
        IEnumerable<GenderList> GenderPagedList(int page, int rows);
    }
}
