﻿using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class WaytoPayRepository : Repository<WaytoPay>, IWaytoPayRepository
    {
        public WaytoPayRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<WaytoPay> GetWaytoPaysByPaymentType(string idPaymentType)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idPaymentType", idPaymentType);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<WaytoPay>("dbo.WayToPayByPaymentType", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
