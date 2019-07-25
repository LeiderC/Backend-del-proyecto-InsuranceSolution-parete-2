using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class BranchOfficeRepository : Repository<BranchOffice>, IBranchOfficeRepository
    {
        public BranchOfficeRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BranchOfficeList> BranchOfficePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<BranchOfficeList>("dbo.BranchOfficePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
