using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentTypeRepository : Repository<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PaymentType> PaymentTypeByPaymentMethod(string paymentMethod)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPaymentMethod", paymentMethod);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PaymentTypeList>("dbo.PaymentTypeByPaymentMethod", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PaymentTypeList> PaymentTypePagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PaymentTypeList>("dbo.PaymentTypePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
