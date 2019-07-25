using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public DateTime ExpiditionDate { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Risk { get; set; }
        public int IdPolicyHolder { get; set; }
        public float Premium { get; set; }

    }
}
