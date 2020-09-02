using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{

    public class ExternalUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Help { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public int? IdCustomer { get; set; }
    }
}