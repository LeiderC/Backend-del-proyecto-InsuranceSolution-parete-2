using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class AllowedDomainsRepository : Repository<AllowedDomains>, IAllowedDomainsRepository
    {
        public AllowedDomainsRepository(string connectionString) : base(connectionString)
        {
        }
    }
}