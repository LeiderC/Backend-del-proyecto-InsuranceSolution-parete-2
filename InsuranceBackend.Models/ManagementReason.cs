using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class ManagementReason
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
