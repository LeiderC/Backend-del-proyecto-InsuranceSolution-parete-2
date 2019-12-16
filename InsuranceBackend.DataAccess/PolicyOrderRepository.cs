using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyOrderRepository : Repository<PolicyOrder>, IPolicyOrderRepository
    {
        public PolicyOrderRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
