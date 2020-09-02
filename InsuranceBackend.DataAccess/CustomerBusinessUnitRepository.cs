using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class CustomerBusinessUnitRepository : Repository<CustomerBusinessUnit>, ICustomerBusinessUnitRepository
    {
        public CustomerBusinessUnitRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<CustomerBusinessUnitList> CustomerBusinessUnitListByCustomer(int idCustomer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerBusinessUnitList>("dbo.CustomerBusinessUnitDetailByCustomer", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CustomerBusinessUnitList> CustomerBusinessUnitPagedList(int page, int rows, int idCustomer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idCustomer", idCustomer);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerBusinessUnitList>("dbo.CustomerBusinessUnitPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}