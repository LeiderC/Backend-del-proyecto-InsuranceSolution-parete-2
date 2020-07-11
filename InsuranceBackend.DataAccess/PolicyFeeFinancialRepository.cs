using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyFeeFinancialRepository : Repository<PolicyFeeFinancial>, IPolicyFeeFinancialRepository
    {
        public PolicyFeeFinancialRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeleteFeeByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyFeeFinancial WHERE IdPolicy = @IdPolicy;";
            using(var connection= new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy});
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        public IEnumerable<PolicyFeeFinancialList> PolicyFeeFinancialListByPolicy(int idPolicy, bool paid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);
            parameters.Add("@paid", paid);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyFeeFinancialList>("dbo.PolicyFeeFinancialListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
