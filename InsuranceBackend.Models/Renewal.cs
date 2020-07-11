using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Renewal
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int IdUser { get; set; }
        public DateTime RenewalDate { get; set; }
    }
}