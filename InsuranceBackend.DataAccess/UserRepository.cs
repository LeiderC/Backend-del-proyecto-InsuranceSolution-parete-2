using Dapper;
using System.Collections.Generic;
using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System.Data.SqlClient;
using System;

namespace InsuranceBackend.DataAccess
{
    public class UserRepository : Repository<SystemUser>, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }
        public SystemUser ValidateUser(string login, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@login", login);
            parameters.Add("@password", password);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<SystemUser>("dbo.ValidateUser", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public SystemUser ValidateUserPassword(string login, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@login", login);

            SystemUser user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var eventName = connection.QueryFirst<string>("SELECT * FROM SystemUser WHERE Id = 1");
                user = connection.QueryFirstOrDefault<SystemUser>("dbo.ValidateUserPassword", parameters,
                     commandType: System.Data.CommandType.StoredProcedure);
            }

            if (user != null)
            {
                if (!Password.PasswordUtil.VerifyPassword(password, user.Password, user.Help))
                    user = null;
            }

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

        public IEnumerable<SystemUser> GetAllUsers(bool salesman)
        {
             var parameters = new DynamicParameters();
            parameters.Add("@salesman", salesman);

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<SystemUser>("dbo.GetAllUsers", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public bool CheckPermissions(int idUser, string menu, string subMenu, string action)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idUser", idUser);
            parameters.Add("@menu", menu);
            parameters.Add("@submenu", subMenu);
            parameters.Add("@action", action);
            using (var connection = new SqlConnection(_connectionString))
            {
                //Permission
                var result = connection.QuerySingle("dbo.CheckPermissions", parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                if (result.Permission == 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
