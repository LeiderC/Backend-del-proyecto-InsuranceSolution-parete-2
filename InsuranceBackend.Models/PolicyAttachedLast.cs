using System;

namespace InsuranceBackend.Models
{
    public class PolicyAttachedLast
    {
        public int Id { get; set; }
        public DateTime ExpiditionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IdPolicyState { get; set; }
        public string IdPaymentMethod { get; set; }
        public int? IdFinancial { get; set; }
        public double FeeValue { get; set; }
        public int FeeNumbers { get; set; }
        public int Payday { get; set; }
        public double InitialFee { get; set; }
        public string IdFinancialOption { get; set; }
        public int? IdSalesMan { get; set; }
        public int? IdExternalSalesMan { get; set; }
        public double PremiumValue { get; set; }
        public double Iva { get; set; }
        public double NetValue { get; set; }
        public double PremiumExtra { get; set; }
        public double TotalValue { get; set; }
        public string IdMovementType { get; set; }
        public int? IdVehicle { get; set; }
        public int? IdUser { get; set; }
        public string License { get; set; }
        public int? IdOnerous { get; set; }
        public string Observation { get; set; }
        public string PendingRegistration { get; set; }
        public string Inspected { get; set; }
        public bool ReqAuthorization { get; set; }
        public double OwnProducts { get; set; }
        public double TotalInitialFee { get; set; }
        public bool RevokePromisoryNote { get; set; }
        public string Certificate { get; set; }
        public string OwnerIdentification { get; set; }
        public string OwnerName { get; set; }
        public double Runt { get; set; }
        public double Contribution { get; set; }
        public int IdPolicyHeader { get; set; }
        public bool ReqAuthorizationFinancOwnProduct { get; set; }
        public string InvoiceNumber { get; set; }
        public int? IdExternalUser { get; set; }
        public int? IdPolicyParent { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}