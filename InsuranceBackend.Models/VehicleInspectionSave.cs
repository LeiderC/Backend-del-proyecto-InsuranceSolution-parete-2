using System.Collections.Generic;

namespace InsuranceBackend.Models
{
    public class VehicleInspectionSave
    {
        public VehicleInspection VehicleInspection { get; set; }
        public List<DigitalizedFile> DigitalizedFiles { get; set; }
    }
}