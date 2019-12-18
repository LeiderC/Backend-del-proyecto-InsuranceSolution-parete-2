using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class UserProfile
    {
        [Key]
        public int IdUser { get; set; }
        public int IdProfile { get; set; }
    }
}
