using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicySettlementRepository : Repository<PolicySettlement>, IPolicySettlementRepository
    {
        public PolicySettlementRepository(string connectionString) : base(connectionString)
        {
        }
    }
}