using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPolicyHeader
    {
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public int IdInsuranceSubline { get; set; }
        public string Number { get; set; }
    }
}
