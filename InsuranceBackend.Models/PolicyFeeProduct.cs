using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyFeeProduct
    {
        public int Id { get; set; }
        public int IdPolicy { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public DateTime? DatePayment { get; set; }
    }
}