using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyInsuredRepository : Repository<PolicyInsured>, IPolicyInsuredRepository
    {
        public PolicyInsuredRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePolicyInsuredByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyInsured WHERE IdPolicy = @IdPolicy;";
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
