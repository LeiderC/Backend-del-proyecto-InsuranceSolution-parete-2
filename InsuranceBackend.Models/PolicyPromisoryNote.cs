using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyPromisoryNote
    {
        public int Id { get; set; }
        public int IdPolicy { get; set; }
        public DateTime CreationDate { get; set; }
        public int IdUser { get; set; }
        public string State { get; set; }

    }
}