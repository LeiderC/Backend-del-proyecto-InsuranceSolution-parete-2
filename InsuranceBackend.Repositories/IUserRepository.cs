using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IUserRepository: IRepository<User>
    {
        User ValidateUser(string email, string password);
    }
}
