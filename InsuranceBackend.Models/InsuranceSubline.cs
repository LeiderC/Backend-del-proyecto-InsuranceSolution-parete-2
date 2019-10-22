﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceSubline
    {
        public int Id { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public string Description { get; set; }
        public float Commission { get; set; }
        public float IVA { get; set; }
    }
}