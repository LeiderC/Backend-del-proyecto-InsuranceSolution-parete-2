using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ITypeDigitalizedFileRepository : IRepository<TypeDigitalizedFile>
    {
        IEnumerable<TypeDigitalizedFileList> TypeDigitalizedFilePagedList(int page, int rows);
    }
}
