using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyPaymentThird
    {
        public int Id { get; set; }
        public int IdPolicy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ThirdPayDate { get; set; }
        public int IdUser { get; set; }
        public string State { get; set; }
        public int IdPaymentMethodThird { get; set; }
        public int IdPaymentAccountThird { get; set; }
        public string PaidDestination { get; set; }
        public int IdPayment { get; set; }
    }
}