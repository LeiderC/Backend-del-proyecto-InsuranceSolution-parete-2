using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyOrderDetail
    {
        public int IdPolicyOrder { get; set; }
        public int IdPolicy { get; set; }
        public DateTime CreationDate { get; set; }
        public string State { get; set; }
    }
}
