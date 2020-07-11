using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetCheckPermissions
    {
        public string Menu { get; set; }
        public string Submenu { get; set; }
        public string Action { get; set; }
    }
}
