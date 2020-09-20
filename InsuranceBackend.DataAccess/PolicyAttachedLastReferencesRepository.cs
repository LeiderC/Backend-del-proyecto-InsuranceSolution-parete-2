using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAttachedLastReferencesRepository : Repository<PolicyAttachedLastReferences>, IPolicyAttachedLastReferencesRepository
    {
        public PolicyAttachedLastReferencesRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePolicyReferenciesByPolicy(int idPolicy)
        {
            string sql = "DELETE PolicyAttachedLastReferences WHERE IdPolicy = @IdPolicy;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPolicy = idPolicy });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

        public IEnumerable<PolicyReferencesList> PolicyReferencesListByBolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyReferencesList>("dbo.PolicyAttachedLastReferencesListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
