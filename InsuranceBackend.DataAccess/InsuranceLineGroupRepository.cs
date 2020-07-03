using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceLineGroupRepository : Repository<InsuranceLineGroup>, IInsuranceLineGroupRepository
    {
        public InsuranceLineGroupRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InsuranceLineGroup> InsuranceLineGroupList(int idInsuranceLine)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsuranceLine", idInsuranceLine);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceLineGroup>("dbo.InsuranceLineGroupList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
