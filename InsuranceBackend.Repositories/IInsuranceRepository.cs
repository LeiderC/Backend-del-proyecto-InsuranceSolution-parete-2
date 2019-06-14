using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IInsuranceRepository: IRepository<Insurance>
    {
            IEnumerable<InsuranceList> InsurancePagedList(int page, int rows);
    }
}
