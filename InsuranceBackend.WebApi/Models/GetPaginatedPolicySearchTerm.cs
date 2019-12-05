using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedPolicySearchTerm
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int IdCustomer { get; set; }
    }
}
