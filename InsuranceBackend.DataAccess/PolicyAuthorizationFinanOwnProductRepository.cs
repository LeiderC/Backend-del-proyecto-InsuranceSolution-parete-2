using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAuthorizationFinanOwnProductRepository : Repository<PolicyAuthorizationFinanOwnProduct>, IPolicyAuthorizationFinanOwnProductRepository
    {
        public PolicyAuthorizationFinanOwnProductRepository(string connectionString) : base(connectionString)
        {
        }
    }
}