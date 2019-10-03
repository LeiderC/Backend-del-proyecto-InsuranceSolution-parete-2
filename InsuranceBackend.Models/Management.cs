using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Management
    {
        public int Id { get; set; }
        public string ManagementType { get; set; }
        public int IdCustomer { get; set; }
        public int IdInsurance { get; set; }
        public int CreationUser { get; set; }
        public string Other { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Hour { get; set; }
        public string State { get; set; }
        public int DelegatedUser { get; set; }
        public string Subject { get; set; }
    }
}