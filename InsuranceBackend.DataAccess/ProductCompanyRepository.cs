using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ProductCompanyRepository : Repository<ProductCompany>, IProductCompanyRepository
    {
        public ProductCompanyRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ProductCompany> ProductByCompany(int idCompany)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCompany", idCompany);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ProductCompany>("dbo.ProductByCompany", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
