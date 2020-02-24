using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPolicyPaymentThirdParties
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IdInsurance { get; set; }
        public int IdFinancial { get; set; }
    }
}
