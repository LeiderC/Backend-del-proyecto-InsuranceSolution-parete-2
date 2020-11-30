using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyInpsectedRepository : Repository<PolicyInspected>, IPolicyInspectedRepository
    {
        public PolicyInpsectedRepository(string connectionString) : base(connectionString)
        {
        }
    }
}