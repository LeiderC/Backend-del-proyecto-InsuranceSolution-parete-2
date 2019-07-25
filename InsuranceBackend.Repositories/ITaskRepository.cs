using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        IEnumerable<TaskList> TaskPagedList(int page, int rows);
    }
}
