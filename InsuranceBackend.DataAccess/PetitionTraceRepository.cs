using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PetitionTraceRepository : Repository<PetitionTrace>, IPetitionTraceRepository
    {
        public PetitionTraceRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PetitionTraceList> PetitionTracePagedList(int page, int rows, int idPetition)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idPetition", idPetition);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PetitionTraceList>("dbo.PetitionTracePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
