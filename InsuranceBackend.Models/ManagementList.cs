using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ManagementList : Management
    {
        public string ManagementTypeDesc { get; set; }
        public string ManagementPartnerDesc { get; set; }
        public string Partner { get; set; }
        public string StateDesc { get; set; }
        public string CreationUserName { get; set; }
        public string DelegatedUserName { get; set; }
        public int TotalDays { get; set; }
        public int TotalRecords { get; set; }
        public string ManagementReasonDesc { get; set; }
        public int TotalExtras { get; set; }
        public bool ReqAuthorization { get; set; }
        public bool Authorized { get; set; }
        public bool ReqAuthorizationFinancOwnProduct { get; set; }
        public bool AuthorizedFinancOwnProduct { get; set; }
        public bool IsOrderCol { get; set; }
    }
}
