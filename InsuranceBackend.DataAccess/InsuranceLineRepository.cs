using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceLineRepository : Repository<InsuranceLine>, IInsuranceLineRepository
    {
        public InsuranceLineRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InsuranceLineList> InsuranceLinePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceLineList>("dbo.InsuranceLinePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InsuranceLine> InsuranceLineCommissionByInsurance(int idInsurance)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsurance", idInsurance);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceLine>("dbo.InsuranceLineCommissionByInsurance", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InsuranceLine> InsuranceLineByInsurance(int idInsurance)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsurance", idInsurance);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceLine>("dbo.InsuranceSublineByInsurance", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
