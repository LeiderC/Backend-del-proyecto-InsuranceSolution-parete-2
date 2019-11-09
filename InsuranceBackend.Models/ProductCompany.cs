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
    }
}
