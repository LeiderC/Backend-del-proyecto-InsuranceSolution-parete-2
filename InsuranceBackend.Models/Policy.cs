using System;

namespace InsuranceBackend.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int IdInsurance { get; set; }
        public int IdInsuranceLine { get; set; }
        public int IdInsuranceSubline { get; set; }
        public DateTime ExpiditionDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IdPolicyHolder { get; set; }
        public string IdPolicyType { get; set; }
        public string IdPolicyState { get; set; }
        public string IdPaymentMethod { get; set; }
        public int? IdFinancial { get; set; }
        public double FeeValue { get; set; }
        public int FeeNumbers { get; set; }
        public int Payday { get; set; }
        public double InitialFee { get; set; }
        public string IdFinancialOption { get; set; }
        public int IdSalesMan { get; set; }
        public int? IdExternalSalesMan { get; set; }
        public double PremiumValue { get; set; }
        public double Iva { get; set; }
        public double NetValue { get; set; }
        public double PremiumExtra { get; set; }
        public double TotalValue { get; set; }
        public string IdMovementType { get; set; }
        public int? IdVehicle { get; set; }
        public int IdUser { get; set; }
        public string License { get; set; }
        public bool IsOrder { get; set; }
        public int? IdOnerous { get; set; }
    }
}