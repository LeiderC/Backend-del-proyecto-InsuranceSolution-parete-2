using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class RelationshipRepository : Repository<Relationship>, IRelationshiprepository
    {
        public RelationshipRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
