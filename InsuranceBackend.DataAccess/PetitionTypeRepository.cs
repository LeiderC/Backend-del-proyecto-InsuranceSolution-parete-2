using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PetitionTypeRepository : Repository<PetitionType>, IPetitionTypeRepository
    {
        public PetitionTypeRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
