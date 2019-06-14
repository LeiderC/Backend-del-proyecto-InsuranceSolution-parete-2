using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class IdentificationTypeRepository : Repository<IdentificationType>, IIdentificationTypeRepository
    {
        public IdentificationTypeRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<IdentificationTypeList> IdentificationTypePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<IdentificationTypeList>("dbo.IdentificationTypePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}

