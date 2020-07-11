using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceLineCommission
    {
        [ExplicitKey]
        public int IdInsurance { get; set; }
        [ExplicitKey]
        public int IdInsuranceLine { get; set; }
        public float Commission { get; set; }
        public float IVA { get; set; }
    }
}