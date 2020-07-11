using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class FinancialOptionRepository : Repository<FinancialOption>, IFinancialOptionRepository
    {
        public FinancialOptionRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
