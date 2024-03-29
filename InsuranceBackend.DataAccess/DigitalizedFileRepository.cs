﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class DigitalizedFileRepository : Repository<DigitalizedFile>, IDigitalizedFileRepository
    {
        public DigitalizedFileRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<DigitalizedFileList> DigitalizedFilePagedList(int idCustomer, int page, int rows, int idPolicyOrder, int idPolicy, int idPayment)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);
            parameters.Add("@idPolicyOrder", idPolicyOrder);
            parameters.Add("@idPolicy", idPolicy);
            parameters.Add("@idPayment", idPayment);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<DigitalizedFileList>("dbo.DigitalizedFilePagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
