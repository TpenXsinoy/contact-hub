using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user">UserCreationDto details to be created</param>
        /// <returns>UserDto details of the newly created user</returns>
        Task<UserDto> CreateUser(UserCreationDto user);

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>UserUIDetailsDto with id the same as the param</returns>
        public Task<UserUIDetailsDto?> GetUserById(Guid id);

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">Username of a user</param>
        /// <returns>UserUIDetailsDto with username the same as the param</returns>
        public Task<UserUIDetailsDto?> GetUserByUsername(string username);

        /// <summary>
        /// Gets a user by username with token details
        /// </summary>
        /// <param name="username">Username of a user</param>
        /// <returns>User with Token details with username the same as the param</returns>
        public Task<UserTokenDto?> GetUserByUsernameWithToken(string username);

        /// <summary>
        /// Checks if a user with the same email exists
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>True if use exist, otherwise false</returns>
        Task<bool> IsUserEmailExist(string email);

        /// <summary>
        /// Checks if a user with the same username exists
        /// </summary>
        /// <param name="username">Username of a user</param>
        /// <returns>True if use exist, otherwise false</returns>
        Task<bool> IsUsernameExist(string username);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="id">Id of user to be updated</param>
        /// <param name="user">Details of user to be updated</param>
        /// <returns>UserUIDetailsDto of the newly updated user</returns>
        public Task<UserUIDetailsDto> UpdateUser(Guid id, UserCreationDto user);

        /// <summary>
        /// Creates a password hash and salt
        /// </summary>
        /// <param name="password">Password to be hashed</param>
        /// <param name="passwordHash">Hashed password</param>
        /// <param name="passwordSalt">Salt password</param>
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        /// <summary>
        /// Verifies a password hash and salt
        /// </summary>
        /// <param name="password">Password to be checked</param>
        /// <param name="passwordHash">Hashed password of a user</param>
        /// <param name="passwordSalt">Salt password of a user</param>
        /// <returns>True if passwords are the same, otherwise false</returns>
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
