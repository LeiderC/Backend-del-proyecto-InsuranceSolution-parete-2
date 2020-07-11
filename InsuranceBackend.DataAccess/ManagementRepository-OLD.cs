using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace InsuranceBackend.DataAccess
{
    public class ManagementRepository : Repository<Management>, IManagementRepository
    {
        public ManagementRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ManagementList> ManagementPagedList(int idCustomer, int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idCustomer", idCustomer);
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                var reader = connection.QueryMultiple("dbo.ManagementPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                var managementList = reader.Read<ManagementList>().ToList();
                var managementTaskList = reader.Read<ManagementTaskList>().ToList();
                foreach(var item in managementList)
                {
                    item.SetTaskList(managementTaskList);
                }

                return managementList;
            }
        }
    }
}
