using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyPaymentThirdRepository : Repository<PolicyPaymentThird>, IPolicyPaymentThirdRepository
    {
        public PolicyPaymentThirdRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
