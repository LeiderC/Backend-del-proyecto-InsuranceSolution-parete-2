using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InsuranceBackend.Models
{
    public class ManagementList : Management
    {
        public int TotalRecords { get; set; }
        public string ManagementType { get; set; }
        public string UserName { get; set; }
        public List<ManagementTaskList> TaskDetails { get; set; }
        public void SetTaskList(List<ManagementTaskList> taskDetails)
        {
            TaskDetails = taskDetails.Where(t => t.IdManagement == Id).ToList();
        }
    }
}
