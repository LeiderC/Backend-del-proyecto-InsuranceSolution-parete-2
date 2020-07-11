using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class IntermediaryRepository : Repository<Intermediary>, IIntermediaryRepository
    {
        public IntermediaryRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<IntermediaryList> IntermediaryPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<IntermediaryList>("dbo.IntermediaryPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
