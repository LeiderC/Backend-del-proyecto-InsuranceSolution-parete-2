using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System;

namespace InsuranceBackend.DataAccess
{
    public class PolicyRepository : Repository<Policy>, IPolicyRepository
    {
        public PolicyRepository(string connectionString) : base(connectionString)
        {
        }

        public Policy PolicyByIdPolicyOrder(int idPolicyOrder, bool isOrder)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyOrder", idPolicyOrder);
            parameters.Add("@isOrder", isOrder);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<Policy>("dbo.PolicyByIdPolicyOrder", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPagedListSearchTerms(string identification, string name, string number, int idcustomer, int iduserpolicyorder, bool isOrder, int page, int rows, string stateOrder, string stateId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@identification", identification);
            parameters.Add("@name", name);
            parameters.Add("@number", number);
            parameters.Add("@idCustomer", idcustomer);
            parameters.Add("@idUserPolicyOrder", iduserpolicyorder);
            parameters.Add("@isOrder", isOrder);
            parameters.Add("@stateOrder", stateOrder);
            parameters.Add("@stateId", stateId);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPagedListSearchTerms", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public PolicyList PolicyListById(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<PolicyList>("dbo.PolicyListByIdPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public PolicyList PolicyAttColListById(int idPolicy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<PolicyList>("dbo.PolicyAttColListByIdPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCustomerPagedListSearchTerms(string type, string searchCriteria, int page, int rows, int idSalesman)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@type", type);
            parameters.Add("@searchCriteria", searchCriteria);
            parameters.Add("@idSalesman", idSalesman);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCustomerPagedListSearchTerms", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public IEnumerable<PolicyList> PolicyCustomerPagedListSearchTermsOnlyPolicy(string type, string searchCriteria, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@type", type);
            parameters.Add("@searchCriteria", searchCriteria);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCustomerPagedListSearchTermsOnlyPolicy", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCustomerPagedListSearchTermsOnlyOrder(string type, string searchCriteria, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@type", type);
            parameters.Add("@searchCriteria", searchCriteria);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCustomerPagedListSearchTermsOnlyOrder", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPromisoryNotePagedList(DateTime startDate, DateTime endDate, int page, int rows, int idFinancial)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);
            parameters.Add("@idFinancial", idFinancial);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPromisoryNotePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyOutlayPagedList(DateTime startDate, DateTime endDate, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyOutlayPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCommissionPagedList(int InsuranceId, DateTime startDate, DateTime endDate, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@insuranceId", InsuranceId);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCommissionPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCommissionList(int InsuranceId, DateTime startDate, DateTime endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@insuranceId", InsuranceId);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCommissionList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCommissionSalesmanList(int idSalesman, DateTime? startDate, DateTime? endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idSalesman", idSalesman);
            parameters.Add("@startDate", startDate.HasValue ? startDate.Value.Date : startDate);
            parameters.Add("@endDate", endDate.HasValue ? endDate.Value.Date : endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCommissionSalesmanList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyPortfolioList> PolicyPortfolioReportList(DateTime? startDate, DateTime? endDate, int idInsurance, int idCustomer, string license)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);
            parameters.Add("@idInsurance", idInsurance);
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@license", license);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyPortfolioList>("dbo.PolicyPortfolioReportList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPaymentThirdParties(DateTime? startDate, DateTime? endDate, int idInsurance, int idFinancial, string type,
            bool paid, int idPaymentMethodThird, int idPaymentThirdAccount)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);
            parameters.Add("@idInsurance", idInsurance);
            parameters.Add("@idFinancial", idFinancial);
            parameters.Add("@type", type);
            parameters.Add("@paid", paid);
            parameters.Add("@idPaymentMethodThird", idPaymentMethodThird);
            parameters.Add("@idPaymentThirdAccount", idPaymentThirdAccount);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPaymentThirParties", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPendingAuthorizationList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPendingAuthorizationList", null,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<dynamic> PolicyReportProduction(int idUser, DateTime startDate, DateTime endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query("dbo.PolicyReportProduction", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyReportProductionConsolidated(int idUser, DateTime startDate, DateTime endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyReportProductionConsolidated", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyOrderReport(int page, int rows, int idUser, DateTime? startDate, DateTime? endDate, bool all)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);
            parameters.Add("@idUser", idUser);
            parameters.Add("@all", all);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyOrderReport", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyOrderListConsolidated> PolicyOrderReportConsolidated(DateTime startDate, DateTime endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyOrderListConsolidated>("dbo.PolicyOrderReportConsolidated", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPaymentIncome(DateTime? startDate, DateTime? endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPaymentIncome", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPaymentAccountReceivable(DateTime? startDate, DateTime? endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPaymentAccount", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyCommissionExternalSalesmanList(int idExternalSalesman, DateTime? startDate, DateTime? endDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idExternalSalesman", idExternalSalesman);
            parameters.Add("@startDate", startDate.HasValue ? startDate.Value.Date : startDate);
            parameters.Add("@endDate", endDate.HasValue ? endDate.Value.Date : endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyCommissionExternalSalesmanList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyHeader()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyListHeader", null,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Policy PolicyHeader(int idInsurance, int idInsuranceLine, int idInsuranceSubline, string number)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idInsurance", idInsurance);
            parameters.Add("@idInsuranceLine", idInsuranceLine);
            parameters.Add("@idInsuranceSubline", idInsuranceSubline);
            parameters.Add("@number", number);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Policy>("dbo.PolicyHeader", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyPendingAuthorizationDiscList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPendingAuthorizationDiscList", null,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyAttached(int idPolicyHeader)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyHeader", idPolicyHeader);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyListAttached", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public bool PolicyDuplicate(int idPolicy, int idInsuranceLine, string license, bool isOrder){
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicy", idPolicy);
            parameters.Add("@idInsuranceLine", idInsuranceLine);
            parameters.Add("@license", license);
            parameters.Add("@isOrder", isOrder);
            using (var connection = new SqlConnection(_connectionString))
            {
                //Exist
                var result = connection.QuerySingle("dbo.PolicyDuplicate", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                if (result.Exist == 0)
                    return false;
                else
                    return true;
            }
        }

        public IEnumerable<PolicyList> PolicyHeaderByIdCustomer(int idCustomer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyListHeaderByCustomer", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyExternalUserByCustomer(int idCustomer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyExternalUserByCustomer", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public PolicyList PolicyAttColListByIdPolicyOrder(int idPolicyOrder)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyOrder", idPolicyOrder);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirst<PolicyList>("dbo.PolicyAttColListByIdPolicyOrder", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyColReportProduction(DateTime startDate, DateTime endDate, int IdPolicyHolder)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);
            parameters.Add("@idPolicyHolder", IdPolicyHolder);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyColProductionReport", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<PolicyList> PolicyAttachedByPolicyAttLast(int idPolicyAttLast)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyAttachedLast", idPolicyAttLast);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyListAttachedByPolicyLast", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
