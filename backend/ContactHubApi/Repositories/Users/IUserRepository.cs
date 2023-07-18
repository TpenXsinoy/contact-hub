using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Users
{
    public interface IUserRepository
    {
        Task<Guid> CreateUser(User user);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByUsername(string username);
        Task<bool> IsUserEmailExist(string email);
        Task<bool> IsUsernameExist(string username);
        Task<bool> UpdateUser(User user);
    }
}
