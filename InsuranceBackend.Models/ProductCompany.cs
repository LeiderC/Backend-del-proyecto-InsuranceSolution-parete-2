using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ProductCompany
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int IdCompany { get; set; }
        public bool Authorization { get; set; }
        public string Alias { get; set; }
        public float Value { get; set; }
        public int IVA { get; set; }
        public float TotalValue { get; set; }
        public float MinimumValue { get; set; }
        public bool Calculate { get; set; }
        public float MinimumValueRenewal { get; set; }
        public string Observation { get; set; }
        public float PercentageCalculate { get; set; }

    }
}
