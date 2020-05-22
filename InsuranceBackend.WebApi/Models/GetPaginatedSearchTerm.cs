using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedSearchTerm
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string SearchTerm { get; set; }
        public string Type { get; set; }
    }
}
