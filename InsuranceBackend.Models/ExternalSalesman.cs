using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ExternalSalesman
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Short { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Movil { get; set; }
        public int? IdCity { get; set; }
        public DateTime? BirthDate { get; set; }
        public string IdentificationNumber { get; set; }
        public string Email { get; set; }
    }
}
