using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<CityList> CityPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CityList>("dbo.CityPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<City> CityByState(int idState)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idState", idState);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<City>("dbo.CityByState", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
