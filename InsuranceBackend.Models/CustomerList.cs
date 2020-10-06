using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class CustomerList : Customer
    {
        public int TotalRecords { get; set; }
        public string IdentificationTypeDesc {get;set;}
        public string CustomerTypeDesc {get;set;}
        public string Name {get;set;}
        public string GenderDesc {get;set;}
        public string MaritalStatusDesc {get;set;}
        public string ResidenceCountryDesc {get;set;}
        public string ResidenceStateDesc {get;set;}
        public string ResidenceCityDesc {get;set;}
        
    }
}
