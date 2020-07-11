using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class SystemAuditRepository : Repository<SystemAudit>, ISystemAuditRepository
    {
        public SystemAuditRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SystemAuditList> SystemAuditPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<SystemAuditList>("dbo.SystemAuditPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
