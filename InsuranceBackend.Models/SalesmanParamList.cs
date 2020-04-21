using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SalesmanParamList: SalesmanParam
    {
        public int TotalRecords { get; set; }
        public string Salesman { get; set; }
        public string MovementType { get; set; }
        public string Insurance { get; set; }
        public string InsuranceLine { get; set; }
        public string InsuranceSubline { get; set; }
        public string SalesmanProfile { get; set; }
        public string SalesTypeDesc { get; set; }
    }
}