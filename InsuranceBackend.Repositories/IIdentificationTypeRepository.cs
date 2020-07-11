using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IIdentificationTypeRepository : IRepository<IdentificationType>
    {
        IEnumerable<IdentificationTypeList> IdentificationTypePagedList(int page, int rows);
    }
}
