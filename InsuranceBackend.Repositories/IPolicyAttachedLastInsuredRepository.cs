﻿using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastInsuredRepository : IRepository<PolicyAttachedLastInsured>
    {
        bool DeletePolicyInsuredByPolicy(int idPolicy);
    }
}
