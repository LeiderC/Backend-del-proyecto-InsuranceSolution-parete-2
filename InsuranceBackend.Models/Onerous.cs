using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Onerous
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public string Digit { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmailRenewals { get; set; }
    }
}
