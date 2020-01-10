using System;
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
        public float ExtraValue { get; set; }
    }
}