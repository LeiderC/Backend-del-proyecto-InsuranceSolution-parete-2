using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IDigitalizedFileRepository : IRepository<DigitalizedFile>
    {
        IEnumerable<DigitalizedFileList> DigitalizedFilePagedList(int page, int rows);
    }
}
