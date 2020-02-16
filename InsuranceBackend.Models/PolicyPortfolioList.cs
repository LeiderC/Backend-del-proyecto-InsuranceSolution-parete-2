using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyPortfolioList: Policy
    {
        public string InsuranceDesc { get; set; }
        public string InsuranceShort { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceLineShort { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public string InsuranceList { get; set; }
        public double Total { get; set; }
        public double TotalPay { get; set; }
        public double Balance { get; set; }
        public double DueBalance { get; set; }
        public int DueDays { get; set; }
        public double UpThirty { get; set; }
        public double BetThirtySixty { get; set; }
        public double BetSixtyNinety { get; set; }
        public double MoreNinety { get; set; }
    }
}
