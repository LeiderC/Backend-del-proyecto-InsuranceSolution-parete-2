using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string License { get; set; }
        public int IdVehicleBrand { get; set; }
        public int IdVehicleClass { get; set; }
        public int IdVehicleReference { get; set; }
        public string Fasecolda { get; set; }
        public string IdVehicleService { get; set; }
        public int Model { get; set; }
        public int Cylinder { get; set; }
        public int Weight { get; set; }
        public string IdVehicleBodywork { get; set; }
        public string Chassis { get; set; }
        public int PassengersNumber { get; set; }
        public int CommercialValue { get; set; }
        public string Class { get; set; }
        public string Brand { get; set; }
        public string Engine { get; set; }
    }
}