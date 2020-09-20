using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAttachedLastInsuredRepository : Repository<PolicyAttachedLastInsured>, IPolicyAttachedLastInsuredRepository
    {
        public PolicyAttachedLastInsuredRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePolicyInsuredByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyAttachedLastInsured WHERE IdPolicy = @IdPolicy;";
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
