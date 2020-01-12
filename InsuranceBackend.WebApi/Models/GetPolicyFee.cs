using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPolicyFee
    {
        public int IdPolicy { get; set; }
        public bool Paid { get; set; }
    }
}
