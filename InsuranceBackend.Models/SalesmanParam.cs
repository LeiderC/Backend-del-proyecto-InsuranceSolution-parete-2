using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SalesmanParam
    {
        public int Id { get; set; }
        public int IdSalesman { get; set; }
        public string IdMovementType { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public int IdInsuranceSubline { get; set; }
        public float Budget { get; set; }
        public float CompliancePercentage { get; set; }
        public float BonusPercentage { get; set; }
        public float BonusPercentageHigher { get; set; }
        public string IdSalesmanProfile { get; set; }
        public string SalesType { get; set; }
    }
}
