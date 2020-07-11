using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyOrderDetailRepository : IRepository<PolicyOrderDetail>
    {
        bool UpdateState(int IdPolicyOrder, string State);
    }
}
