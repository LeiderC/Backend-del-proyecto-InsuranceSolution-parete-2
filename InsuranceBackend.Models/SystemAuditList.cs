using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SystemAuditList : SystemAudit
    {
        public string UserDesc { get; set; }
        public string ActionDesc { get; set; }
        public string ProcessDesc { get; set; }
        public string SubMenuDesc { get; set; }
        public string MasterMenuDesc { get; set; }
        public int TotalRecords { get; set; }
    }
}
