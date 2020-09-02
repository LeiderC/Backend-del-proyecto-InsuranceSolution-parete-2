using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetSearchTerm
    {
        public string SearchTerm { get; set; }
        public bool IsOrder { get; set; }
    }
}
