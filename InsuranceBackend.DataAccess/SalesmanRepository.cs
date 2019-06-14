using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class SalesmanRepository : Repository<Salesman>, ISalesmanRepository
    {
        public SalesmanRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SalesmanList> SalesmanPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<SalesmanList>("dbo.SalesmanPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
