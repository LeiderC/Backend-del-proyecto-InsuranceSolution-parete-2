using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ManagementRepository : Repository<Management>, IManagementRepository
    {
        public ManagementRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ManagementList> ManagementPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementList>("dbo.ManagementPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ManagementExtraList> ManagementExtraPagedList(int page, int rows, int idManagementParent)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idManagementParent", idManagementParent);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementExtraList>("dbo.ManagementExtraPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
