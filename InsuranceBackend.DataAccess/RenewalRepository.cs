using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class RenewalRepository : Repository<Renewal>, IRenewalRepository
    {
        public RenewalRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
