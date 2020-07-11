using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Short { get; set; }
        public string LineClass { get;set;}
        public string LineType { get; set; }
        public string State { get; set; }
        public bool IvaExempt { get; set; }
}
}
