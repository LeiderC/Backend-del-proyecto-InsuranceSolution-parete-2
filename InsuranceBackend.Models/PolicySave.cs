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
        public List<BeneficiaryList> PolicyBeneficiaries { get; set; }
        public List<PolicyFee> PolicyFees { get; set; }
        public int PolicyOrderId { get; set; }
        public List<PolicyReferences> PolicyReferences { get; set; }
    }
}
