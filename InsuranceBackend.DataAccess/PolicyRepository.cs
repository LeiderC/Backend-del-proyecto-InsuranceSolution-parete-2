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

        public Policy PolicyByIdPolicyOrder(int idPolicyOrder)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyOrder", idPolicyOrder);

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

        public IEnumerable<PolicyList> PolicyPagedListSearchTerms(string identification, string name, string number, int idcustomer, int iduserpolicyorder, bool isOrder, int page, int rows)
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

        public IEnumerable<PolicyList> PolicyCustomerPagedListSearchTerms(string type, string searchCriteria, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@type", type);
            parameters.Add("@searchCriteria", searchCriteria);

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

        public IEnumerable<PolicyList> PolicyPromisoryNotePagedList(DateTime startDate, DateTime endDate, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@startDate", startDate);
            parameters.Add("@endDate", endDate);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<PolicyList>("dbo.PolicyPromisoryNotePagedList", parameters,
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

        public IEnumerable<PolicyPortfolioList> PortfolioReportList(DateTime? startDate, DateTime? endDate, int idInsurance, int idCustomer, string license)
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
    }
}
