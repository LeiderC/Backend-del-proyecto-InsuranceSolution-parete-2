using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedDigitalizedFile
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public int IdCustomer { get; set; }
        public int IdPolicyOrder { get; set; }
        public int IdPolicy { get; set; }
        public int IdPayment { get; set; }
    }
}
