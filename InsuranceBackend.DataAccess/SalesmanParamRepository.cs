using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class SalesmanParamRepository : Repository<SalesmanParam>, ISalesmanParamRepository
    {
        public SalesmanParamRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SalesmanParamList> SalesmanParamPagedList(int page, int rows, int idSalesman)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idSalesman", idSalesman);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<SalesmanParamList>("dbo.SalesmanParamPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
