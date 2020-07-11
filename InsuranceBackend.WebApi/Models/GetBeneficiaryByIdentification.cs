using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetBeneficiaryByIdentification
    {
        public int IdentificationType { get; set; }
        public string Identification { get; set; }
    }
}
