using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ManagementTaskList
    {
        public int IdManagement { get; set; }
        public int IdTask { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime AlertDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UserName { get; set; }
    }
}
