using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Default { get; set; }
        public string PhoneCode { get; set; }
    }
}
