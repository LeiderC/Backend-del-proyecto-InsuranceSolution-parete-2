using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyAttachedLastFee
    {
        public int Id { get; set; }
        public int IdPolicy { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public DateTime? DatePayment { get; set; }
        public DateTime? DateInsurance { get; set; }
        public double ValueOwnProduct { get; set; }
    }
}