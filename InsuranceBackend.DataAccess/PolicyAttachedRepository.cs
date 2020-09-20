using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAttachedRepository : Repository<PolicyAttachedLast>, IPolicyAttachedLastRepository
    {
        public PolicyAttachedRepository(string connectionString) : base(connectionString)
        {
        }

        public PolicyAttachedLast PolicyAttachedLastByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<PolicyAttachedLast>("dbo.PolicyAttachedLastByIdPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}