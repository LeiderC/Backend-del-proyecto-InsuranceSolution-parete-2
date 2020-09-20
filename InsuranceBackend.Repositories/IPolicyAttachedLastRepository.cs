using InsuranceBackend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyAttachedLastRepository : IRepository<PolicyAttachedLast>
    {
        PolicyAttachedLast PolicyAttachedLastByPolicy(int idPolicy);
    }
}
