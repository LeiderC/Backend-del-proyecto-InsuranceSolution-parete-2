using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SystemUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Write(false)]
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Help { get; set; }
        public string Roles { get; set; }
        [Write(false)]
        public int IdProfile { get; set; }
        public bool ChangePassword { get; set; }
        public string State { get; set; }
        public bool Authorizing { get; set; }
        public int IdSalesman { get; set; }
        public bool CancelOrders { get; set; }
    }
}
