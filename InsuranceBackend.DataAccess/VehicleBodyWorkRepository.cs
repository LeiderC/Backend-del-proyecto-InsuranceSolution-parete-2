using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class VehicleBodyWorkRepository : Repository<VehicleBodywork>, IVehicleBodyworkRepository
    {
        public VehicleBodyWorkRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
