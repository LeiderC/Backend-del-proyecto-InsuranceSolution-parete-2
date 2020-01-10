using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyProductRepository : Repository<PolicyProduct>, IPolicyProductRepository
    {
        public PolicyProductRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePolicyProductByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyProduct WHERE IdPolicy = @IdPolicy;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        public IEnumerable<PolicyProductList> PolicyProductListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyProductList>("dbo.PolicyProductListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
