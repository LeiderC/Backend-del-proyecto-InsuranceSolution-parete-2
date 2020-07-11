﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ExternalSalesmanRepository : Repository<ExternalSalesman>, IExternalSalesmanRepository
    {
        public ExternalSalesmanRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ExternalSalesmanList> ExternalSalesmanPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ExternalSalesmanList>("dbo.ExternalSalesmanPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
