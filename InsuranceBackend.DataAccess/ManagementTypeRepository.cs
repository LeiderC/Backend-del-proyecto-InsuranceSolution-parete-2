using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ManagementTypeRepository : Repository<ManagementType>, IManagementTypeRepository
    {
        public ManagementTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
