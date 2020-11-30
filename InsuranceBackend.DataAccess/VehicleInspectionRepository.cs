using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace InsuranceBackend.DataAccess
{
    public class VehicleInspectionRepository : Repository<VehicleInspection>, IVehicleInspectionRepository
    {
        public VehicleInspectionRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
