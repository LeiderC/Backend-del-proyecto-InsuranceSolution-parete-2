using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;

namespace InsuranceBackend.DataAccess
{
    public class ExternalUserRepository : Repository<ExternalUser>, IExternalUserRepository
    {
        public ExternalUserRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ExternalUserList> GetExternalUserLists()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<ExternalUserList>("dbo.ExternalUserList", null,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public ExternalUser ValidateUserPassword(string login, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@login", login);

            ExternalUser user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                user = connection.QueryFirstOrDefault<ExternalUser>("dbo.ValidateExternalUserPassword", parameters,
                     commandType: System.Data.CommandType.StoredProcedure);
            }

            if (user != null)
            {
                if (!Password.PasswordUtil.VerifyPassword(password, user.Password, user.Help))
                    user = null;
            }

            return user;
        }
    }
}
