using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class VehicleClassRepository : Repository<VehicleClass>, IVehicleClassRepository
    {
        public VehicleClassRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
