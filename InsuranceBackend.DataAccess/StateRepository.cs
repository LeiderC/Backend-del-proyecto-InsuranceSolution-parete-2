using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<StateList> StatePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<StateList>("dbo.StatePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<State> StateByCountry(int idCountry)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCountry", idCountry);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<State>("dbo.StateByCountry", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
