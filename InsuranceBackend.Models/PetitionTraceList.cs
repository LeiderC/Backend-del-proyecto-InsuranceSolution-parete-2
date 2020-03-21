using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PetitionTraceList: PetitionTrace
    {
        public int TotalRecords { get; set; }
        public string UserName { get; set; }
    }
}