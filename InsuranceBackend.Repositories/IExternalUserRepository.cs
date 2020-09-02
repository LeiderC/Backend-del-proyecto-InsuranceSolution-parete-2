using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IExternalUserRepository : IRepository<ExternalUser>
    {
        ExternalUser ValidateUserPassword(string login, string password);
        IEnumerable<ExternalUserList> GetExternalUserLists();
    }
}
