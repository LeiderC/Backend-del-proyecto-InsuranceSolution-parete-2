using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class RestrictedPhonesRepository : Repository<RestrictedPhones>, IRestrictedPhonesRepository
    {
        public RestrictedPhonesRepository(string connectionString) : base(connectionString)
        {
        }
    }
}