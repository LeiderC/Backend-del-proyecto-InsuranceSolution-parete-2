using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IManagementRepository : IRepository<Management>
    {
        IEnumerable<ManagementList> ManagementByUserList(int idUser, int idRenewal);
        IEnumerable<ManagementList> ManagementReportByUserList(int idUser, int idRenewal);
        IEnumerable<ManagementList> ManagementPagedList(int page, int rows, int idUser);
        IEnumerable<ManagementExtraList> ManagementExtraPagedList(int page, int rows, int idManagementParent);
        Management ManagementByPolicyOrder(int idPolicyOrder, string managementType);
        IEnumerable<ManagementList> ManagementByCustomerList(int page, int rows, int idCustomer, string state);
    }
}
