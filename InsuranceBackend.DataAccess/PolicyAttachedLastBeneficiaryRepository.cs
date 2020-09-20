using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAttachedLastBeneficiaryRepository : Repository<PolicyAttachedLastBeneficiary>, IPolicyAttachedLastBeneficiaryRepository
    {
        public PolicyAttachedLastBeneficiaryRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePolicyBeneficiaryByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyAttachedLastBeneficiary WHERE IdPolicy = @IdPolicy;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }
    }
}
