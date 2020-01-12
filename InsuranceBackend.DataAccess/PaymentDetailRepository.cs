using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentDetailRepository : Repository<PaymentDetail>, IPaymentDetailRepository
    {
        public PaymentDetailRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePaymentDetailByPayment(int idPayment)
        {
            string sql = "DELETE PaymentDetail WHERE IdPayment = @IdPayment;";
            using (var connection = new SqlConnection(_connectionString))
            {
                int affectedRows = connection.Execute(sql, new { IdPayment = idPayment });
                if (affectedRows > 0)
                    return true;
                return false;
            }
        }

    }
}
