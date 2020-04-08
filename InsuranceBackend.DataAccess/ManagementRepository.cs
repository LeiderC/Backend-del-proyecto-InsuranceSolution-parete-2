﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ManagementRepository : Repository<Management>, IManagementRepository
    {
        public ManagementRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ManagementList> ManagementByUserList(int idUser, int idRenewal)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@idRenewal", idRenewal);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementList>("dbo.ManagementByUser", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ManagementList> ManagementPagedList(int page, int rows, int idUser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idUser", idUser);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementList>("dbo.ManagementPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ManagementExtraList> ManagementExtraPagedList(int page, int rows, int idManagementParent)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idManagementParent", idManagementParent);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementExtraList>("dbo.ManagementExtraPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Management ManagementByPolicyOrder(int idPolicyOrder, string managementType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPolicyOrder", idPolicyOrder);
            parameters.Add("@managementType", managementType);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Management>("dbo.ManagementByPolicyOrder", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<ManagementList> ManagementByCustomerList(int page, int rows, int idCustomer, string state)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@state", state);
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ManagementList>("dbo.ManagementByCustomer", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
