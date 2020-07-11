using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyStateRepository : Repository<PolicyState>, IPolicyStateRepository
    {
        public PolicyStateRepository(string connectionString) : base(connectionString)
        {
        }
    }
}