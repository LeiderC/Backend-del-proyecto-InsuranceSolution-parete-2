using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<PaymentDetailList> PaymentDetailListByPayment(int idPayment)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPayment", idPayment);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PaymentDetailList>("dbo.PaymentDetailListByPayment", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PaymentList> PaymentPagedListSearchTerms(string paymentType, int paymentNumber, int idCustomer, int idPolicy, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@paymentType", paymentType);
            parameters.Add("@paymentNumber", paymentNumber);
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PaymentList>("dbo.PaymentPagedListSearchTerms", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PaymentDetailList> PaymentDetailListByPolicy(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PaymentDetailList>("dbo.PaymentDetailListByPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
