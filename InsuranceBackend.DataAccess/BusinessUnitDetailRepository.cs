using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class BusinessUnitDetailRepository : Repository<BusinessUnitDetail>, IBusinessUnitDetailRepository
    {
        public BusinessUnitDetailRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BusinessUnitDetailList> BusinessUnitDetailListsBySalesman(int idSalesman)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idSalesman", idSalesman);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<BusinessUnitDetailList>("dbo.BusinessUnitDetailListBySalesman", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<BusinessUnitDetailList> BusinessUnitDetailPagedList(int page, int rows, int idBusinessUnit)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idBusinessUnit", idBusinessUnit);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<BusinessUnitDetailList>("dbo.BusinessUnitDetailPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}