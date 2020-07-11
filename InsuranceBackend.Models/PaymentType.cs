using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentType
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public int Number { get; set; }
    }
}
