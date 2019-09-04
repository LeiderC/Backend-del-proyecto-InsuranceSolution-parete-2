using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace InsuranceBackend.DataAccess
{
    public class ManagementTaskRepository : Repository<ManagementTask>, IManagementTaskRepository
    {
        public ManagementTaskRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
