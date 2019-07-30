using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User ValidateUser(string login, string password);
        User ValidateUserPassword(string login, string password);
    }
}
