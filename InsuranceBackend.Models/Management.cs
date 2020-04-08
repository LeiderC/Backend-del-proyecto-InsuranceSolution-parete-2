using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Management
    {
        public int Id { get; set; }
        public string ManagementType { get; set; }
        public string ManagementPartner { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdInsurance { get; set; }
        public int CreationUser { get; set; }
        public string Other { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Hour { get; set; }
        public string State { get; set; }
        public int? DelegatedUser { get; set; }
        public string Subject { get; set; }
        public bool IsExtra { get; set; }
        [Write(false)]
        public int IdManagementParent { get; set; }
        public int? IdPolicyOrder { get; set; }
        public int? IdPayment { get; set; }
        public string IdManagementReason { get; set; }
        public bool? Assignable { get; set; }
        public bool? IsRenewal { get; set; }
    }
}