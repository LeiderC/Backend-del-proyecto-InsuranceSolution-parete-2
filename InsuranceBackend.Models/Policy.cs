using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public int IdInsuranceSubline { get; set; }
        public DateTime ExpiditionDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IdPolicyHolder { get; set; }
        public string IdPolicyType { get; set; }
        public string IdPolicyState { get; set; }
        public string IdPaymentMethod { get; set; }
    }
}
