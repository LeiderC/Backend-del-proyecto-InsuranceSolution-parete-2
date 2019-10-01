using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceRepository : Repository<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InsuranceList> InsurancePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceList>("dbo.InsurancePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Insurance> InsuranceByCommission()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceList>("dbo.InsuranceByCommission", 
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Insurance> InsuranceBySubline()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceList>("dbo.InsuranceBySubline",
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
