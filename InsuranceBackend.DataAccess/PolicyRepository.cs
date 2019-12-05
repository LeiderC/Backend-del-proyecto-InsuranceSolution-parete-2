using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyRepository : Repository<Policy>, IPolicyRepository
    {
        public PolicyRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PolicyList> PolicyPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPagedListSearchTerms(string identification, string name, string number, int idcustomer, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@identification", identification);
            parameters.Add("@name", name);
            parameters.Add("@number", number);
            parameters.Add("@idCustomer", idcustomer);

            using (var connection = new SqlConnection(_connectionString))
            {
                List<PolicyList> lst = connection.Query<PolicyList>("dbo.PolicyPagedListSearchTerms", parameters,
                    commandType: System.Data.CommandType.StoredProcedure).AsList<PolicyList>();
                return connection.Query<PolicyList>("dbo.PolicyPagedListSearchTerms", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
