using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PolicyPendingRegistrationSave
    {
        public PolicyPendingRegistration PolicyPendingRegistration {get;set;}
        public DigitalizedFile DigitalizedFile { get; set; }
    }
}