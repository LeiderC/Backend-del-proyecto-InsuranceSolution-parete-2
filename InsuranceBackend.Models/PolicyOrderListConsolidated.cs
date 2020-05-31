using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyOrderListConsolidated
    {
        public string SalesmanName { get; set; }
        public int Pending { get; set; }
        public int Opened { get; set; }
        public int Process { get; set; }
        public int Closed { get; set; }
        public int Total { get; set; }
    }
}
