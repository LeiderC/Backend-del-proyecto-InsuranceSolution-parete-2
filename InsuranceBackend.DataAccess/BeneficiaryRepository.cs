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

        public IEnumerable<BeneficiaryList> BeneficiaryAttachedListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<BeneficiaryList>("dbo.BeneficiaryAttachedListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Beneficiary BeneficiaryByIdentification(string identification, int idIdentificationType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idIdentificationType", idIdentificationType);
            parameters.Add("@identification", identification);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Beneficiary>("dbo.BeneficiaryByIdentification", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<BeneficiaryList> BeneficiaryListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<BeneficiaryList>("dbo.BeneficiaryListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
