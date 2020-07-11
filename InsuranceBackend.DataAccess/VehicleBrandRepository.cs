using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class VehicleBrandRepository : Repository<VehicleBrand>, IVehicleBrandRepository
    {
        public VehicleBrandRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
