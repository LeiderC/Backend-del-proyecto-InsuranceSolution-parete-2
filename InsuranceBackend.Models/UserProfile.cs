using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class UserProfile
    {
        [ExplicitKey]
        public int IdUser { get; set; }
        [ExplicitKey]
        public int IdProfile { get; set; }
    }
}
