using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace InsuranceBackend.DataAccess
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(string connectionString) : base(connectionString)
        {
        }

        public Vehicle VehicleByLicense(string license)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@license", license);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<Vehicle>("dbo.VehicleByLicense", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return result.AsList<Vehicle>().FirstOrDefault();
            }
            return null;
        }
    }
}
