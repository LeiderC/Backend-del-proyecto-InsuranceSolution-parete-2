using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyInsuredRepository : Repository<PolicyInsured>, IPolicyInsured
    {
        public PolicyInsuredRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
