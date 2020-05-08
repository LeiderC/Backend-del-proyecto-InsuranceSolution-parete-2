using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class FasecoldaRepository : Repository<Fasecolda>, IFasecoldaRepository
    {
        public FasecoldaRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Fasecolda> FasecoldaByCode(string code)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Fasecolda>("dbo.FasecoldaByCode", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public FasecoldaDetail FasecoldaDetailByCodeYear(string code, int year)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);
            parameters.Add("@year", year);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<FasecoldaDetail>("dbo.FasecoldaDetailByCodeYear", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
