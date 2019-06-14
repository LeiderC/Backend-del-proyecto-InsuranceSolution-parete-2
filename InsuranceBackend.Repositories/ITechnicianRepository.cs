using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ITechnicianRepository : IRepository<Technician>
    {
        IEnumerable<TechnicianList> TechnicianPagedList(int page, int rows);
    }
}
