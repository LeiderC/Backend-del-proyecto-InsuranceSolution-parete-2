using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PetitionRepository : Repository<Petition>, IPetitionRepository
    {
        public PetitionRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PetitionList> PetitionPagedList(int page, int rows, int idCustomer, string state)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@state", state);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PetitionList>("dbo.PetitionPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
