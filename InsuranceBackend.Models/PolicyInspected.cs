using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyInspected
    {
        [ExplicitKey]
        public int IdPolicy { get; set; }
        public int IdUser { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Observation { get; set; }
        public DateTime InspectedDate { get; set; }
    }
}