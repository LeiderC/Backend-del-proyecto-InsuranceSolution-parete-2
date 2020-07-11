using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SystemAudit
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string IdAction { get; set; }
        public string Process { get; set; }
        public string Detail { get; set; }
        public string IdSystemSubMenu { get; set; }
        public DateTime Date { get; set; }
    }
}
