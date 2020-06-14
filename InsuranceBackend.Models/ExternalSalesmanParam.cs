using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ExternalSalesmanParam
    {
        public int Id { get; set; }
        public int IdExternalSalesman { get; set; }
        public string IdMovementType { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public int IdInsuranceSubline { get; set; }
        public float BonusPercentage { get; set; }
    }
}
