﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class PolicyBeneficiaryRepository : Repository<PolicyBeneficiary>, IPolicyBeneficiary
    {
        public PolicyBeneficiaryRepository(string connectionString) : base(connectionString)
        {
        }

    }
}
