using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyList : Policy
    {
        public string Name { get; set; }
        public string InsuranceDesc { get; set; }
        public string InsuranceShort { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceLineShort { get; set; }
        public string InsuranceSublineDesc { get; set; }
        public string State { get; set; }
        public string InsuranceList { get; set; }
        public string BeneficiariesList { get; set; }
        public string MovementShort { get; set; }
        public string PaymentMethodShort { get; set; }
        public string PaymentMethodDesc { get; set; }
        public string FinancialDesc { get; set; }
        public string SalesmanName { get; set; }
        public string ExternalSalesmanName { get; set; }
        public int IdPolicyOrder { get; set; }
        public int TotalRecords { get; set; }
        public string Phone { get; set; }
        public string Movil { get; set; }
        public string IdentificationNumber { get; set; }
        public string Email { get; set; }
        public int IdCustomer { get; set; }
        public bool Leaflet { get; set; }
        public string StateOrder { get; set; }
        public string StateOrderDesc { get; set; }
        public double CommissionPercentage { get; set; }
        public double Commission { get; set; }
        public int RowNumber { get; set; }
        public bool GenerateInterestDue { get; set; }
        public double? Value { get; set; }
        public DateTime? DatePayment { get; set; }
        public string SalesType { get; set; }
        public double TotalOwnProducts { get; set; }
        public bool HasPaid { get; set; }
        public string IdPaymentType { get; set; }
        public double InitialFeePaid { get; set; }
        public double OwnProductsPaid { get; set; }
        public int IdPayment { get; set; }
        public DateTime DatePaymentThird { get; set; }
        public string PaymentMethodThird { get; set; }
        public string Account { get; set; }
        public string Tecnico { get; set; }
        public DateTime? OrderCreationDate { get; set; }
        public DateTime? OrderClosingDate { get; set; }
    }
}
