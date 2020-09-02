using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentDetailProduct
    {
        public int Id { get; set; }
        public int IdPayment { get; set; }
        public int IdPolicy { get; set; }
        public float Value { get; set; }
        public int FeeNumber { get; set; }
    }
}
