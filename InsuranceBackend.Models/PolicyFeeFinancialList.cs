using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyFeeFinancialList : PolicyFeeFinancial
    {
        public int TotalRecords { get; set; }
        public bool Paid { get; set; }
        public string PaidDesc { get; set; }
        public bool Expire { get; set; }
        public double DueInterestValue { get; set; }
    }
}