using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ExternalSalesmanParamList : ExternalSalesmanParam
    {
        public int TotalRecords { get; set; }
        public string ExternalSalesman { get; set; }
        public string MovementType { get; set; }
        public string Insurance { get; set; }
        public string InsuranceLine { get; set; }
        public string InsuranceSubline { get; set; }
    }
}