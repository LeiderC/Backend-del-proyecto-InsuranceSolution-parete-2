using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class SystemProfileRepository : Repository<SystemProfile>, ISystemProfileRepository
    {
        public SystemProfileRepository(string connectionString) : base(connectionString)
        {
        }
    }
}