using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyFeeRepository : Repository<PolicyFee>, IPolicyFeeRepository
    {
        public PolicyFeeRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeleteFeeByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyFee WHERE IdPolicy = @IdPolicy;";
            using(var connection= new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy});
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        public bool DeleteFeeByPolicyFeeNumber(int idPolicy, int feeNumber)
        {
            string sql = "DELETE PolicyFee WHERE IdPolicy = @IdPolicy AND Number =@FeeNumber;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy, FeeNumber = feeNumber });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        public IEnumerable<PolicyFeeList> PolicyFeeListByPolicy(int idPolicy, bool paid)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);
            parameters.Add("@paid", paid);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyFeeList>("dbo.PolicyFeeListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
