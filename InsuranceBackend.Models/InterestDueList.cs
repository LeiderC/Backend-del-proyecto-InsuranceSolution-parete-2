using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class InterestDueList: InterestDue
    {
        public int TotalRecords { get; set; }
    }
}
