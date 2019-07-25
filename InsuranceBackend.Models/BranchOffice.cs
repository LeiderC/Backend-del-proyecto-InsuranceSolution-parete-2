using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class BranchOffice
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Short { get; set; }
        public string NIT { get; set; }
        public string Digit { get; set; }
        public int IdCity { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Movil { get; set; }
        public string State { get; set; }
    }
}
