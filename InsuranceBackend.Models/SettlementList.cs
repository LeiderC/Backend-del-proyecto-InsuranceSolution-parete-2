using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SettlementList: Settlement
    {
        public int TotalRecords { get; set; }
        public string InsuranceDesc { get; set; }
        public string InsuranceShort { get; set; }
        public string StateDesc { get; set; }
        public string UserSettleName { get; set; }
        public string UserSettlePayName { get; set; }
    }
}