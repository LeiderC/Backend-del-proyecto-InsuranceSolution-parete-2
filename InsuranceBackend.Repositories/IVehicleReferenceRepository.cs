using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IVehicleReferenceRepository : IRepository<VehicleReference>
    {
        IEnumerable<VehicleReference> VehicleReferenceByBrand(int idVehicleBrand);
    }
}
