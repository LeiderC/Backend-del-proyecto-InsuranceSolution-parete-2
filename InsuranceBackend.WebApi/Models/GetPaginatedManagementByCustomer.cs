using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedManagementByCustomer
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public int IdCustomer { get; set; }
        public string State { get; set; }
    }
}
