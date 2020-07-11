using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class CancellationReasonRepository : Repository<CancellationReason>, ICancellationReasonRepository
    {
        public CancellationReasonRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<CancellationReasonList> CancellationReasonPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CancellationReasonList>("dbo.CancellationPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
