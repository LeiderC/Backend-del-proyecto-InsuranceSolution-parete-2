using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class BeneficiaryList : Beneficiary
    {
        public double Percentage { get; set; }
        public string IdentificationTypeDesc { get; set; }
        public string GenderDesc { get; set; }
        public string RelationshipDesc { get; set; }
        public int TotalRecords { get; set; }
    }
}
