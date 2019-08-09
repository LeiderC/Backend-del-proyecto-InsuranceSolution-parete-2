using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceLineClassRepository : Repository<InsuranceLineClass>, IInsuranceLineClassRepository
    {
        public InsuranceLineClassRepository(string connectionString) : base(connectionString)
        {
        }
    }
}