using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyRepository : Repository<Policy>, IPolicyRepository
    {
        public PolicyRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PolicyList> PolicyPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
