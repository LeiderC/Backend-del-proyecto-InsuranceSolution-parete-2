﻿using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IUserRepository: IRepository<SystemUser>
    {
        SystemUser ValidateUser(string login, string password);
        SystemUser ValidateUserPassword(string login, string password);
        IEnumerable<UserList> UserPagedList(int page, int rows);
        IEnumerable<SystemUser> GetAllUsers();
    }
}
