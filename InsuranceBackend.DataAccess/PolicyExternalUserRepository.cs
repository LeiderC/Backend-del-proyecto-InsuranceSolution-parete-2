using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyExternalUserRepository : Repository<PolicyExternalUser>, IPolicyExternalUserRepository
    {
        public PolicyExternalUserRepository(string connectionString) : base(connectionString)
        {
        }
    }
}