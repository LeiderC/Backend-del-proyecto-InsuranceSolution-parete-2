using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicySettlementSave
    {
        public List<PolicyList> Policies { get; set; }
        public Settlement Settlement { get; set; }
    }
}
