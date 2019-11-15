using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetSalesmanCommsisionSearchTerm
    {
        public int SalesmanId { get; set; }
        public int? InsuranceId { get; set; }
        public int? InsuranceLineId { get; set; }
        public int? InsuranceSublineId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
