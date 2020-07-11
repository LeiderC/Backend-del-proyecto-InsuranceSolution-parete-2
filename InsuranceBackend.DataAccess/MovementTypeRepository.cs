using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class MovementTypeRepository : Repository<MovementType>, IMovementTypeRepository
    {
        public MovementTypeRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
