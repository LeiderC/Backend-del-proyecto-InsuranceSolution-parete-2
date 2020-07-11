using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InterestDue
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public double Value { get; set; }
    }
}
