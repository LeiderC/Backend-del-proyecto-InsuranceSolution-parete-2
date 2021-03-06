﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedPolicySearchTerm
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int IdCustomer { get; set; }
        public bool FindByUserPolicyOrder { get; set; }
        public bool IsOrder { get; set; }
        public int IdUser { get; set; }
        public string StateOrder { get; set; }
        public string IdPolicyState { get; set; }
    }
}
