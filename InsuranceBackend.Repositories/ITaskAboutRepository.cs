using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ITaskAboutRepository : IRepository<TaskAbout>
    {
        IEnumerable<TaskAboutList> TaskAboutPagedList(int page, int rows);
    }
}
