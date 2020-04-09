using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class SubMenuProfilePerm
    {
        public int Id { get; set; }
        public string IdSubMenu { get; set; }
        public int IdUser { get; set; }
        public int IdUserAllowed { get; set; }
    }
}