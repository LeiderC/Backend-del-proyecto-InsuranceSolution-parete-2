using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{

    public class ExternalUserList : ExternalUser
    {
        public string CustomerName { get; set; }
        public int TotalRecords { get; set; }
    }
}