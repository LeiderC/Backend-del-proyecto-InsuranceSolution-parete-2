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
        public string Type { get; set; }
        public bool Paid { get; set; }
        public int IdSalesman { get; set; }
        public int IdPaymentMethodThird { get; set; }
        public int IdAccountThird { get; set; }
        public bool Inspected { get; set; }
        public bool Register { get; set; }
    }
}
