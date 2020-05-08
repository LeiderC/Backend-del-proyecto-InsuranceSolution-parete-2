using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Fasecolda
    {
        [ExplicitKey]
        public string Code { get; set; }
        public string Brand { get; set; }
        public string Class { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public int Weight { get; set; }
        public string IdVehicleService { get; set; }
        public string GearboxType { get; set; }
        public int Cylinder { get; set; }
        public int PassengersNumber { get; set; }
        public int LoadingCapacity { get; set; }
        public int Doors { get; set; }
        public bool AirConditioning { get; set; }
        public int Axes { get; set; }
        public string State { get; set; }
        public string Fuel { get; set; }
        public string Transmission { get; set; }
    }
}