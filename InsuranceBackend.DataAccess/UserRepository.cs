using Dapper;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }
        public User ValidateUser(string login, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@login", login);
            parameters.Add("@password", password);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<User>("dbo.ValidateUser", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public User ValidateUserPassword(string login, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@login", login);

            User user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                user = connection.QueryFirstOrDefault<User>("dbo.ValidateUserPassword", parameters,
                     commandType: System.Data.CommandType.StoredProcedure);
            }

            if (!Password.PasswordUtil.VerifyPassword(password, user.Password, user.Help))
                user = null;

            return user;
        }

    }
}
