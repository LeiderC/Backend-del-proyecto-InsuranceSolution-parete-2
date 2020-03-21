using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class SettlementRepository : Repository<Settlement>, ISettlementRepository
    {
        public SettlementRepository(string connectionString) : base(connectionString)
        {
        }
    }
}