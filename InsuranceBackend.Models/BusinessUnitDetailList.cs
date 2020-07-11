using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class BusinessUnitDetailList: BusinessUnitDetail
    {
        public int TotalRecords { get; set; }
        public string BusinessUnit { get; set; }
        public string Salesman { get; set; }
    }
}