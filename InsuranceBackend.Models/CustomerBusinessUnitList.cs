using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class CustomerBusinessUnitList : CustomerBusinessUnit
    {
        public string Customer { get; set; }
        public string Salesman { get; set; }
        public string BusinessUnit { get; set; }
        public string StateDesc { get; set; }
        public int IdSalesman { get; set; }
        public int TotalRecords { get; set; }
        public bool ShowAll { get; set; }
    }
}