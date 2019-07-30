using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess.Password
{
    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
