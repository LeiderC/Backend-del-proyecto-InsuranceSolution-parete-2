﻿using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyProductRepository : IRepository<PolicyProduct>
    {
        bool DeletePolicyProductByPolicy(int idPolicy);
        IEnumerable<PolicyProductList> PolicyProductListByPolicy(int idPolicy);
        IEnumerable<PolicyProductList> PolicyAttachedProductListByPolicy(int idPolicy);
    }
}
