using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System.Linq;

namespace InsuranceBackend.DataAccess
{
    public class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(string connectionString) : base(connectionString)
        {
        }

        public UserProfile UserProfileByUser(int idUser)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Query<UserProfile>("dbo.UserProfileByUser", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                return result.AsList<UserProfile>().FirstOrDefault();
            }
        }
    }
}
