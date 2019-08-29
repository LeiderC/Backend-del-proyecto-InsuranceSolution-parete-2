using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceLineCommissionRepository : Repository<InsuranceLineCommission>, IInsuranceLineCommissionRepository
    {
        public InsuranceLineCommissionRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InsuranceLineCommissionList> InsuranceLineCommissionPagedList(int idInsuranceLine, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsuranceLine", idInsuranceLine);
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceLineCommissionList>("dbo.InsuranceLineCommissionPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public InsuranceLineCommission InsuranceLineCommissionSingle(int idInsurance, int idInsuranceLine)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsurance", idInsurance);
            parameters.Add("@idInsuranceLine", idInsuranceLine);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QuerySingle<InsuranceLineCommission>("dbo.GetInsuranceLineCommission", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
