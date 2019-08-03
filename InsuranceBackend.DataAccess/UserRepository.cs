using Dapper;
using System.Collections.Generic;
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

        public IEnumerable<UserList> UserPagedList(int page, int rows)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@page", page);
            parameters.Add("@rows", rows);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<UserList>("dbo.UserPagedList", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
