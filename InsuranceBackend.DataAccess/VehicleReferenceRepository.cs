using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class VehicleReferenceRepository : Repository<VehicleReference>, IVehicleReferenceRepository
    {
        public VehicleReferenceRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<VehicleReference> VehicleReferenceByBrand(int idVehicleBrand)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idVehicleBrand", idVehicleBrand);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<VehicleReference>("dbo.VehicleReferenceByBrand", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
