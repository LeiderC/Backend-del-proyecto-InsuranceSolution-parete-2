using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Management
    {
        public int Id { get; set; }
        public string Details { get; set; }
        public int IdManagementType { get; set; }
        public int? IdCustomer { get; set; }
        public int IdUser { get; set; }
        public DateTime CreationDate { get;set;}
        public DateTime? EndDate { get; set; }

    }
}