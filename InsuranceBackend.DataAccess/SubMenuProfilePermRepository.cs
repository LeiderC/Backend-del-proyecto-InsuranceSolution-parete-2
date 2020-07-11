using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class SubMenuProfilePermRepository : Repository<SubMenuProfilePerm>, ISubMenuProfilePermRepository
    {
        public SubMenuProfilePermRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SubMenuProfilePerm> SubMenuProfilePermListByUser(string idSubmenu, int idUser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@idSubMenu", idSubmenu);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<SubMenuProfilePerm>("dbo.SubMenuProfilePermListByUser", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
