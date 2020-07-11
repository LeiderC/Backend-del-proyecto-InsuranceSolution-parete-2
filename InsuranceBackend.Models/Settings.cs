using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Settings
    {
        public string Id { get; set; }
        public double SalesmanDirectBussPerc { get; set; }
        public double SalesmanCompPerc { get; set; }
        public double SalesmanJrGraceMonth { get; set; }
        public double IVA { get; set; }
        public double IVAS { get; set; }
        public int TechnicalUserId { get; set; }
        public int SettlementNumber { get; set; }
        public int PetitionsNumber { get; set; }
        public int LastTechnicalUserId { get; set; }
        public string PaymentDisclaimer { get; set; }
    }
}