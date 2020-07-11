using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InsuranceList : Insurance
    {
        public string NameState { get; set; }
        public int TotalRecords { get; set; }
    }
}
