using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IBusinessUnitRepository : IRepository<BusinessUnit>
    {
        IEnumerable<BusinessUnitList> BusinessUnitPagedList(int page, int rows);

    }
}
