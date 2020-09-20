using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<CustomerList> CustomerPagedList(int page, int rows, string searchTerm)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@searchTerm", searchTerm);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerList>("dbo.CustomerPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CustomerList> CustomerByIdentificationNumber(string identificationNumber, int idSalesman, string type)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@identificationNumber", identificationNumber);
            parameters.Add("@idSalesman", idSalesman);
            parameters.Add("@type", type);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerList>("dbo.CustomerByIdentificationNumber", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CustomerList> InsuredListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerList>("dbo.InsuredListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Customer CustomerByIdentificationNumber(string identificationNumber)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@identificationNumber", identificationNumber);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Customer>("dbo.CustomerAnyByIdentificationNumber", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<CustomerList> InsuredAttachedListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<CustomerList>("dbo.InsuredAttachedListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
