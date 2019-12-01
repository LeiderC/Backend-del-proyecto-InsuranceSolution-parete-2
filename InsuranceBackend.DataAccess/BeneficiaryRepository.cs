using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class BeneficiaryRepository : Repository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(string connectionString) : base(connectionString)
        {
        }

        public Beneficiary BeneficiaryByIdentification(string identification)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@identification", identification);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Beneficiary>("dbo.BeneficiaryByIdentification", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
