using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        public int IdPayment { get; set; }
        public int IdPolicy { get; set; }
        public float Value { get; set; }
        public int FeeNumber { get; set; }
        public float DueInterestValue { get; set; }
        public bool InitialFee { get; set; }
        public bool WithoutFee { get; set; }
        public DateTime? DatePayFinancial { get; set; }
        public double ValueOwnProduct { get; set; }
        public double ValueDiscount { get; set; }
    }
}
