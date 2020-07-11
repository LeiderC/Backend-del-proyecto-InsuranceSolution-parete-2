using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Petition
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int IdCustomer { get; set; }
        public int IdUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public string State { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}