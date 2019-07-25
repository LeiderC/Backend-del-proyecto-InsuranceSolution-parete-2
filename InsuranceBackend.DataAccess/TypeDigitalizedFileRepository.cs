using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class TypeDigitalizedFileRepository : Repository<TypeDigitalizedFile>, ITypeDigitalizedFileRepository
    {
        public TypeDigitalizedFileRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<TypeDigitalizedFileList> TypeDigitalizedFilePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<TypeDigitalizedFileList>("dbo.TypeDigitalizedFilePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
