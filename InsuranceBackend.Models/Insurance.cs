﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Insurance
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Short { get; set; }
        public string NIT { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public bool Weekly { get; set; }
        public int Weekday { get; set; }
        public int Day1 { get; set; }
        public int Day2 { get; set; }
        public bool SupportsDirect { get; set; }
        public bool VehicleInspect { get; set; }
        public string EmailVehicleInspect { get; set; }
        public string InspectCenter { get; set; }
    }
}
