using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyReferences
    {
        public int Id { get; set; }
        public int IdPolicy { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int IdRelationship { get; set; }
    }
}
