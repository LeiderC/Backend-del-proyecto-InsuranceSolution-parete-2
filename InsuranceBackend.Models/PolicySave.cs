using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicySave
    {
        public Policy Policy { get; set; }
        public List<PolicyProduct> PolicyProducts { get; set; }
    }
}
