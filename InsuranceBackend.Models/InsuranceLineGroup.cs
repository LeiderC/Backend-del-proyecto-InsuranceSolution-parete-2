﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceLineGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdInsuranceLine { get; set; }
    }
}
