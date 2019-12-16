using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicySave
    {
        public Policy Policy { get; set; }
        public List<PolicyProduct> PolicyProducts { get; set; }
        public Vehicle Vehicle { get; set; }
        public List<Customer> PolicyInsured { get; set; }
        public List<Beneficiary> PolicyBeneficiaries { get; set; }
        public int PolicyOrderId { get; set; }
    }
}
