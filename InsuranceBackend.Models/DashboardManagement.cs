using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.Models
{
    public class DashboardManagement
    {
        public int Total { get; set; }
        public int Urgent { get; set; }
        public int Important { get; set; }
        public int Finished { get; set; }
        public int WithoutPromissoryNote { get; set; }
    }
}
