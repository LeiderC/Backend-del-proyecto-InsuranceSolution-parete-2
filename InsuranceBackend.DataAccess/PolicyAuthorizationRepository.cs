using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAuthorizationRepository : Repository<PolicyAuthorization>, IPolicyAuthorizationRepository
    {
        public PolicyAuthorizationRepository(string connectionString) : base(connectionString)
        {
        }
    }
}