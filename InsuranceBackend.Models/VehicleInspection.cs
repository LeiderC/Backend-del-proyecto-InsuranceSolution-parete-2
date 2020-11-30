using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class VehicleInspection
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MiddleLastName { get; set; }
        public int IdInsurance { get; set; }
        public string License { get; set; }
        public string Chasis {get;set;}
        public string Motor {get;set;}
        public string IdVehicleService {get;set;}
        public string Observation {get;set;}
        public string IdVehicleType {get;set;}
    }
}