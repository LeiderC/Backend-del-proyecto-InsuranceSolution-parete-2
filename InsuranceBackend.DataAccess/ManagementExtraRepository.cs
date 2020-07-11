using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ManagementExtraRepository : Repository<ManagementExtra>, IManagementExtraRepository
    {
        public ManagementExtraRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
