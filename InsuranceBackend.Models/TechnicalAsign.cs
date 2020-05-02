using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class TechnicalAsign
    {
        [ExplicitKey]
        public int IdUser { get; set; }
        public int OrderAssign { get; set; }
    }
}
