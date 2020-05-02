using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class TechnicalAssignRepository : Repository<TechnicalAsign>, ITechnicalAsignRepository
    {
        public TechnicalAssignRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
