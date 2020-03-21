using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicySettlement
    {
        public int Id { get; set; }
        public int IdPolicy { get;set; }
        public int IdSettle { get; set; }
    }
}