using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyProductList: PolicyProduct
    {
        public int TotalRecords { get; set; }
        public string ProductDesc { get; set; }
        public string CompanyDesc { get; set; }
        public string AuthorizationDesc { get; set; }

    }
}