using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPolicyPortfolio
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IdInsurance { get; set; }
        public int IdCustomer { get; set; }
        public string License { get; set; }
    }
}
