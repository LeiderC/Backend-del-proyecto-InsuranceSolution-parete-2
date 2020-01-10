using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Financial
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public bool InitialFeeIncluded { get; set; }
    }
}
