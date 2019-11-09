using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyProductRepository : Repository<PolicyProduct>, IPolicyProductRepository
    {
        public PolicyProductRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
