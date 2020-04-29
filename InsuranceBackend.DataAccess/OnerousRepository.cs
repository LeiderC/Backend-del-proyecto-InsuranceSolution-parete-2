using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class OnerousRepository : Repository<Onerous>, IOnerousRepository
    {
        public OnerousRepository(string connectionString) : base(connectionString)
        {
        }
    }
}