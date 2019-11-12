using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceLineList : InsuranceLine
    {
        public string NameType { get; set; }
        public string NameClass { get; set; }
        public string NameState { get; set; }
        public int TotalRecords { get; set; }
        public string IvaExemptDesc { get; set; }
    }
}
