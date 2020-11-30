using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentReportList
    {
        public string IdPaymentType { get; set; }
        public int Consecutivo { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatePayment { get; set; }
        public double TotalReceived { get; set; }
        public string InsuranceDesc { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public string License { get; set; }
        public string Number { get; set; }
        public string IdentificationOwner { get; set; }
        public string Name { get; set; }
        public string IdentificationInsurance { get; set; }
        public string InsuranceList { get; set; }
        public string BeneficiariesList { get; set; }
        public string SalesmanName { get; set; }
        public string ExternalSalesmanName { get; set; }
        public double TotalValue { get; set; }
        public string PaymentMethodDesc { get; set; }
        public string FinancialDesc { get; set; }
        public string UserName { get; set; }
        public string Observation { get; set; }
    }
}