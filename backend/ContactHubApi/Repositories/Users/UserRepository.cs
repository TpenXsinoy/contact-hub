using ContactHubApi.Context;
using ContactHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactHubApi.Repositories.Users
{
    public class UserRepository : IUserRepository
    {

        private readonly ContactHubContext _dbContext;

        public UserRepository(ContactHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<bool> IsUserEmailExist(string email)
        {
            return _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public Task<bool> IsUsernameExist(string username)
        {
            return _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _dbContext.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
