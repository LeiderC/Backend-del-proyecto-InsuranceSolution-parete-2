using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ManagementExtraList
    {
        public int IdManagement { get; set; }
        public int IdManagementExtra { get; set; }
        public string ManagementType { get; set; }
        public string ManagementPartner { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdInsurance { get; set; }
        public string State { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ManagementTypeDesc { get; set; }
        public string ManagementPartnerDesc { get; set; }
        public string Partner { get; set; }
        public string StateDesc { get; set; }
        public string CreationUserName { get; set; }
        public string DelegatedUserName { get; set; }
        public string Subject { get; set; }
        public int TotalRecords { get; set; }
    }
}
