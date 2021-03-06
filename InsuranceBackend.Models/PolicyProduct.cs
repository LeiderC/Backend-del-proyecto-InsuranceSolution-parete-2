﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyProduct
    {
        public int IdPolicy { get; set; }
        public int IdProduct { get; set; }
        public float Value { get; set; }
        public bool Authorization { get; set; }
        public float? ExtraValue { get; set; }
        public float IVA { get; set; }
        public float TotalValue { get; set; }
        public float FeeValue { get; set; }
        public int FeeNumber { get; set; }
    }
}