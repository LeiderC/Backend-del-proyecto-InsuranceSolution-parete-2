using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
