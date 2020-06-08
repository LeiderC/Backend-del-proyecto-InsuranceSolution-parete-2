using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentDetailFinancialList : PaymentDetailFinancial
    {
        public string Number { get; set; } 
        public string InsuranceDesc { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string License { get; set; }
        public string MovementShort { get; set; }
        public string PaymentTypeDesc { get; set; }
        public int PaymentNumber { get; set; }
        public DateTime DatePayment { get; set; }
        public bool InitialFee { get; set; }
        public bool WithoutFee { get; set; }
        public DateTime? DatePayFinancial { get; set; }
        public double ValueOwnProduct { get; set; }
    }
}
