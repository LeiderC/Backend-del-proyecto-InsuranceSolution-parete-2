using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceSublineRepository : Repository<InsuranceSubline>, IInsuranceSublineRepository
    {
        public InsuranceSublineRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<InsuranceSubline> InsuranceSublineList(int idInsurance, int idInsuranceLine)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsurance", idInsurance);
            parameters.Add("@idInsuranceLine", idInsuranceLine);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceSubline>("dbo.InsuranceSublineList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<InsuranceSublineList> InsuranceSublinePagedList(int idInsuranceLine, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsuranceLine", idInsuranceLine);
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<InsuranceSublineList>("dbo.InsuranceSublinePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        //public InsuranceSubline InsuranceSublineSingle(int id, int idInsurance, int idInsuranceLine)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@id", id);
        //    parameters.Add("@idInsurance", idInsurance);
        //    parameters.Add("@idInsuranceLine", idInsuranceLine);

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        return connection.QuerySingle<InsuranceSubline>("dbo.GetInsuranceSubline", parameters,
        //            commandType: System.Data.CommandType.StoredProcedure);
        //    }
        //}

    }
}
