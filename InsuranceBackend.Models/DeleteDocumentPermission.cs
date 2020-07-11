using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class DeleteDocumentPermission
    {
        [ExplicitKey]
        public int IdUser { get; set; }
        public int Module { get; set; }
        public string State { get; set; }
    }
}
