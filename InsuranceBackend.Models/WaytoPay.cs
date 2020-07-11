using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class WaytoPay
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string IdPaymentType { get; set; }
        public double AditionalCharge { get; set; }
    }
}
