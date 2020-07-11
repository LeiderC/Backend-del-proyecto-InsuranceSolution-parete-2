using InsuranceBackend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Repositories
{
    public interface IDeleteDocumentPermissionRepository : IRepository<DeleteDocumentPermission>
    {
        List<DeleteDocumentPermission> DeleteDocumentPermissionByUserNModule(int idUser, int module);
    }
}
