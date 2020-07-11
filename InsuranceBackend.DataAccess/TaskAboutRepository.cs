using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class TaskAboutRepository : Repository<TaskAbout>, ITaskAboutRepository
    {
        public TaskAboutRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<TaskAboutList> TaskAboutPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<TaskAboutList>("dbo.TaskAboutPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
