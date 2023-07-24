using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Tokens
{
    public interface ITokenService
    {
        /// <summary>
        /// Creates a token for a user to use for authentication
        /// </summary>
        /// <param name="user">User to be authenticated</param>
        /// <returns>Generated token</returns>
        string CreateToken(UserTokenDto user);

        /// <summary>
        ///  Creates a refresh token for a user to use for authentication
        /// </summary>
        /// <returns>RefreshToken</returns>
        RefreshToken GenerateRefreshToken();

        /// <summary>
        /// Verifies if token is still valid
        /// </summary>
        /// <param name="refreshToken">Current refresh token</param>
        /// <param name="user">Current logged in user</param>
        /// <returns> Valid, Invalid, or Expired</returns>
        string Verify(string refreshToken, UserTokenDto user);
    }
}
