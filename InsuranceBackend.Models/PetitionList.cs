using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PetitionList: Petition
    {
        public int TotalRecords { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
    }
}