using System;
using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IPolicyRepository : IRepository<Policy>
    {
        Policy PolicyByIdPolicyOrder(int idPolicyOrder);
        IEnumerable<PolicyList> PolicyPagedList(int page, int rows);
        IEnumerable<PolicyList> PolicyPagedListSearchTerms(string identification, string name, string number, int idcustomer, int iduserpolicyorder, bool isOrder, int page, int rows);
        IEnumerable<PolicyList> PolicyCustomerPagedListSearchTerms(string type, string searchCriteria, int page, int rows, int idSalesman);
        IEnumerable<PolicyList> PolicyCustomerPagedListSearchTermsOnlyPolicy(string type, string searchCriteria, int page, int rows);
        IEnumerable<PolicyList> PolicyCustomerPagedListSearchTermsOnlyOrder(string type, string searchCriteria, int page, int rows);
        PolicyList PolicyListById(int idPolicy);
        IEnumerable<PolicyList> PolicyPromisoryNotePagedList(DateTime startDate, DateTime endDate, int page, int rows);
        IEnumerable<PolicyList> PolicyOutlayPagedList(DateTime startDate, DateTime endDate, int page, int rows);
        IEnumerable<PolicyList> PolicyCommissionPagedList(int InsuranceId, DateTime startDate, DateTime endDate, int page, int rows);
        IEnumerable<PolicyList> PolicyCommissionList(int InsuranceId, DateTime startDate, DateTime endDate);
        IEnumerable<PolicyPortfolioList> PolicyPortfolioReportList(DateTime? startDate, DateTime? endDate, int idInsurance, int idCustomer, string license);
        IEnumerable<PolicyList> PolicyPaymentThirdParties(DateTime? startDate, DateTime? endDate, int idInsurance, int idFinancial);
        IEnumerable<PolicyList> PolicyCommissionSalesmanList(int idSalesman, DateTime? startDate, DateTime? endDate);
        IEnumerable<PolicyList> PolicyPendingAuthorizationList();
        IEnumerable<dynamic> PolicyReportProduction(int idUser, DateTime startDate, DateTime endDate);
        IEnumerable<PolicyList> PolicyReportProductionConsolidated(int idUser, DateTime startDate, DateTime endDate);
        IEnumerable<PolicyList> PolicyOrderReport(int page, int rows, int idUser, DateTime? startDate, DateTime? endDate, bool all);
        IEnumerable<PolicyOrderListConsolidated> PolicyOrderReportConsolidated(DateTime startDate, DateTime endDate);
    }
}
