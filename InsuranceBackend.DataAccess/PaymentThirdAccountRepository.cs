using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PaymentThirdAccountRepository : Repository<PaymentThirdAccount>, IPaymentThirdAccountRepository
    {
        public PaymentThirdAccountRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
