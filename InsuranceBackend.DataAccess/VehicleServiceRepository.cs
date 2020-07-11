using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class VehicleServiceRepository : Repository<VehicleService>, IVehicleServiceRepository
    {
        public VehicleServiceRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
