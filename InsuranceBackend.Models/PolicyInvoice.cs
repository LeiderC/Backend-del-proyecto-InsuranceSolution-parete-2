using System;

namespace InsuranceBackend.Models
{
    public class PolicyInvoice
    {
        public int IdPolicy { get; set; }
        public string InvoiceNumber { get; set; }
        public string IdPaymentMethod { get; set; }
        public int? IdFinancial { get; set; }
        public float? FeeValue { get; set; }
        public int? FeeNumbers { get; set; }
        public int? Payday { get; set; }
        public double PremiumValue { get; set; }
        public double Iva { get; set; }
        public double NetValue { get; set; }
        public double TotalValue { get; set; }
    }
}
