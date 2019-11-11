using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class SalesmanProfileRepository : Repository<SalesmanProfile>, ISalesmanProfileRepository
    {
        public SalesmanProfileRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
