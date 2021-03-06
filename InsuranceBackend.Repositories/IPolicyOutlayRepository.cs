﻿using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyOutlayRepository : IRepository<PolicyOutlay>
    {
        bool DeletePolicyOutlayByPayment(int idPayment);
    }
}
