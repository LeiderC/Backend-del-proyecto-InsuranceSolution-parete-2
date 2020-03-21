using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Settlement
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime CreationDate { get; set; }
        public int IdInsurance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
        public DateTime DateSettle { get; set; }
        public int IdUserSettle { get; set; }
        public DateTime? DatePay { get;set; }
        public int? IdUserPay { get; set; }
        public double Total { get; set; }
    }
}