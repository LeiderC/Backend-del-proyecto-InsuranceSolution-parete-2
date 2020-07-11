using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyTypeRepository : Repository<PolicyType>, IPolicyTypeRepository
    {
        public PolicyTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}