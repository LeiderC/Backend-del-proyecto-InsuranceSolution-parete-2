using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class CustomerBusinessUnit
    {
        [ExplicitKey]
        public int IdCustomer { get; set; }
        [ExplicitKey]
        public int IdBusinessUnitDetail { get; set; }
        [ExplicitKey]
        public string Year { get; set; }
        public string State { get; set; }
    }
}