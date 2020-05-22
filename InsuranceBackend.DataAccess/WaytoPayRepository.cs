using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class WaytoPayRepository : Repository<WaytoPay>, IWaytoPayRepository
    {
        public WaytoPayRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
