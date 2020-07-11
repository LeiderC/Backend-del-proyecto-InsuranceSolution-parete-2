using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class CollectionMethodRepository : Repository<CollectionMethod>, ICollectionMethodRepository
    {
        public CollectionMethodRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
