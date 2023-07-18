using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Users
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(UserCreationDto user);
        public Task<UserUIDetailsDto?> GetUserById(Guid id);
        public Task<UserUIDetailsDto?> GetUserByUsername(string username);
        public Task<UserTokenDto?> GetUserByUsernameWithToken(string username);
        Task<bool> IsUserEmailExist(string email);
        Task<bool> IsUsernameExist(string username);
        public Task<UserUIDetailsDto> UpdateUser(Guid id, UserCreationDto user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
