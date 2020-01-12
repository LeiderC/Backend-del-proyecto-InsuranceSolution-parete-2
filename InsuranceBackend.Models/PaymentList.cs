using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentList : Payment
    {
        public int TotalRecords { get; set; }
        public string PaymentTypeDesc { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string StateDesc { get; set; }
    }
}