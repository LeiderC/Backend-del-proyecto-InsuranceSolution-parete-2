using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class UserList : SystemUser
    {
        public int IdProfile { get; set; }
        public string ProfileName { get; set; }
        public int TotalRecords { get; set; }
    }
}