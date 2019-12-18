using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IDigitalizedFileRepository : IRepository<DigitalizedFile>
    {
        IEnumerable<DigitalizedFileList> DigitalizedFilePagedList(int idCustomer, int page, int rows, int idPolicyOrder, int idPolicy);
    }
}
