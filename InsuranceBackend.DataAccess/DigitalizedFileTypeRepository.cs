using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class DigitalizedFileTypeRepository : Repository<DigitalizedFileType>, IDigitalizedFileTypeRepository
    {
        public DigitalizedFileTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
