using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyPendingRegistrationRepository : Repository<PolicyPendingRegistration>, IPolicyPendingRegistrationRepository
    {
        public PolicyPendingRegistrationRepository(string connectionString) : base(connectionString)
        {
        }
    }
}