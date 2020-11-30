using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class OnerousRepository : Repository<Onerous>, IOnerousRepository
    {
        public OnerousRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<OnerousList> OnerousPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<OnerousList>("dbo.OnerousPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}