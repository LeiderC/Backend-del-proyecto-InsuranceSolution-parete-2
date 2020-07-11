using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class RenewalRepository : Repository<Renewal>, IRenewalRepository
    {
        public RenewalRepository(string connectionString) : base(connectionString)
        {
        }

        public DashboardRenewal DashboardRenewal(int idRenewal)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idRenewal", idRenewal);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<DashboardRenewal>("dbo.DashboardRenewal", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
