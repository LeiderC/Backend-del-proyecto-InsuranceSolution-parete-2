using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System;

namespace InsuranceBackend.DataAccess
{
    public class PolicyBckRepository : Repository<PolicyBck>, IPolicyBckRepository
    {
        public PolicyBckRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
