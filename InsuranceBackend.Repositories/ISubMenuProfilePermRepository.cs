using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface ISubMenuProfilePermRepository : IRepository<SubMenuProfilePerm>
    {
        IEnumerable<SubMenuProfilePerm> SubMenuProfilePermListByUser(string idSubmenu, int idUser);
    }
}
