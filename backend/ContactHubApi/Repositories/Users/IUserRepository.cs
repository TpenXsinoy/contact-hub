using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Users
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user">User details</param>
        /// <returns>Id of the newly created user</returns>
        Task<Guid> CreateUser(User user);

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User with id the same as the param</returns>
        Task<User?> GetUserById(Guid id);

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">Username of a user</param>
        /// <returns>User with username the same as the param</returns>
        Task<User?> GetUserByUsername(string username);

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
        /// <param name="user">Details of user to be updated</param>
        /// <returns>True if update is successful, otherwise false</returns>
        Task<bool> UpdateUser(User user);
    }
}
