using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.Models
{
    public class DashboardRenewal
    {
        public double Budget { get; set; }
        public double VrPremium { get; set; }
        public int TotalRenewals { get; set; }
        public int TotalRenewed { get; set; }
    }
}
