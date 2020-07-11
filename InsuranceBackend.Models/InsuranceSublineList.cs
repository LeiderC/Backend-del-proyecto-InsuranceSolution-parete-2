using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceSublineList : InsuranceSubline
    {
        public string InsuranceDesc { get; set; }
        public string InsuranceShort { get; set; }
        public string InsuranceLineDesc { get; set; }
        public string InsuranceLineShort { get; set; }
        public int TotalRecords { get; set; }
    }
}
