using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentDetailFinancialRepository : Repository<PaymentDetailFinancial>, IPaymentDetailFinancialRepository
    {
        public PaymentDetailFinancialRepository(string connectionString) : base(connectionString)
        {
        }

        public bool DeletePaymentDetailFinancialByPayment(int idPayment)
        {
            string sql = "DELETE PaymentDetailFinancial WHERE IdPayment = @IdPayment;";
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
