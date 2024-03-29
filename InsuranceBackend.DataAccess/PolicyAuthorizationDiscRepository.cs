﻿using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyAuthorizationDiscRepository : Repository<PolicyAuthorizationDisc>, IPolicyAuthorizationDiscRepository
    {
        public PolicyAuthorizationDiscRepository(string connectionString) : base(connectionString)
        {
        }
    }
}