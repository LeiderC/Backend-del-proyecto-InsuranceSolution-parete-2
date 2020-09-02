using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPolicyReportProduction
    {
        public int IdUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool CurrentUser { get; set; }
        public int IdPolicyHolder { get; set; }
    }
}
