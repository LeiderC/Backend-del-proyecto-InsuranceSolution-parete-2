using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Financial
    {
        public int Id { get; set; }
        public string NIT { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool InitialFeeIncluded { get; set; }
        public bool GenerateInterestDue { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int ReferencesNumber { get; set; }
    }
}