using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyBeneficiary
    {
        public int IdPolicy { get; set; }
        public int IdBeneficiary { get; set; }
        public double Percentage { get; set; }

    }
}
