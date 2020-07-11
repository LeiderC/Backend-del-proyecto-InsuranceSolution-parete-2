using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedPolicyCommission
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public int InsuranceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
