using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyPromisoryNoteRepository : Repository<PolicyPromisoryNote>, IPolicyPromisoryNoteRepository
    {
        public PolicyPromisoryNoteRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
