using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentMethodThirdRepository : Repository<PaymentMethodThird>, IPaymentMethodThirdRepository
    {
        public PaymentMethodThirdRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
