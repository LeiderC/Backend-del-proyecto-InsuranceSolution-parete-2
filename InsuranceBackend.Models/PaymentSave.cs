using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentSave
    {
        public Payment Payment { get; set; }
        public List<PaymentDetailList> PaymentDetails { get; set; }
        public List<int> Digitals { get; set; }
    }
}
