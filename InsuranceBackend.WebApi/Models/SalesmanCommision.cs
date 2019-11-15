using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.WebApi.Models
{
    public class SalesmanCommision
    {
        public int SalesmanId { get; set; }
        public string SalesmanIdentifictionNumber { get; set; }
        public string SalesmanName { get; set; }
        public string InsuranceDesc { get; set; }
        public string InsuranceShort { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceLineShort { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public string InsuranceSublineShort { get; set; }
        public double PremiumValue { get; set; }
        public double CommissionValue { get; set; }
    }
}
