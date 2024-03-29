﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InterestDueRepository : Repository<InterestDue>, IInterestDueRepository
    {
        public InterestDueRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InterestDueList> InterestDuePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InterestDueList>("dbo.InterestDuePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public bool ValidateInterestDue(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.QueryFirstOrDefault("dbo.InterestDueValidation", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                if (result.Count > 0)
                    return false;
            }
            return true; 
        }
    }
}
