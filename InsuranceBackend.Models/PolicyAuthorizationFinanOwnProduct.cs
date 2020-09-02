using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyAuthorizationFinanOwnProduct
    {
        [ExplicitKey]
        public int IdPolicy { get; set; }
        public int IdUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Observation { get; set; }
    }
}