using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentDetailList: PaymentDetail
    {
        public string Number { get; set; } 
        public string InsuranceDesc { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string License { get; set; }
        public string MovementShort { get; set; }

    }
}
