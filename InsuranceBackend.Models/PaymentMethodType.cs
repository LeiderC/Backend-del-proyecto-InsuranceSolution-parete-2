using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentMethodType
    {
        [ExplicitKey]
        public string IdPaymentType { get; set; }
        [ExplicitKey]
        public string IdPaymentMethod { get; set; }
    }
}
