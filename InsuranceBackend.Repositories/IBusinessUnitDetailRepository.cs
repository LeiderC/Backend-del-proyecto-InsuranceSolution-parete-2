using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IBusinessUnitDetailRepository : IRepository<BusinessUnitDetail>
    {
        IEnumerable<BusinessUnitDetailList> BusinessUnitDetailPagedList(int page, int rows, int idBusinessUnit);
        IEnumerable<BusinessUnitDetailList> BusinessUnitDetailListsBySalesman(int idSalesman);
    }
}
