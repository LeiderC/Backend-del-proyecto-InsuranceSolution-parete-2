using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyOrderDetailRepository : Repository<PolicyOrderDetail>, IPolicyOrderDetailRepository
    {
        public PolicyOrderDetailRepository(string connectionString) : base(connectionString)
        {
        }

        public bool UpdateState(int IdPolicyOrder, string State)
        {
            string sql = "UPDATE PolicyOrderDetail SET State = @State WHERE IdPolicyOrder = @IdPolicyOrder;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicyOrder = IdPolicyOrder, State = State });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }
    }
}
