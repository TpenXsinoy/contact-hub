using ContactHubApi.Dtos.Tokens;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Services.Tokens;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactHubApi.Controllers
{
    [Route("api/tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ILogger<TokensController> _logger;
        private static UserTokenDto? _user = new();
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokensController(
            ILogger<TokensController> logger,
            ITokenService tokenService,
             IUserService takerService)
        {
            _logger = logger;
            _tokenService = tokenService;
            _userService = takerService;
        }

        /// <summary>
        /// Generates an access token
        /// </summary>
        /// <param name="request">The user to be authenticated with a token</param>
        /// <returns>Returns an access token</returns>
        /// <remarks>
        /// Sample Request:
        ///     
        ///     POST /api/tokens/acquire
        ///     {
        ///         "username": "john123",
        ///         "password": "John123!"
        ///     }
        ///    
        /// </remarks>
        /// 
        /// <response code="200">Successfully acquired an access token</response>
        /// <response code="400">Details are invalid</response>
        /// <response code="404">User is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("acquire")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcquireToken([FromBody] UserLoginDto request)
        {
            try
            {
                var user = await _userService.GetUserByUsername(request.Username!);

                if (user == null)
                {
                    return NotFound($"User {request.Username} is not found");
                }

                if (!_userService.VerifyPasswordHash(request.Password!, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong password!");
                }

                _user!.Id = user.Id;
                _user.FirstName = user.FirstName;
                _user.LastName = user.LastName;
                _user.Username = user.Username;
                _user.Email = user.Email;

                string accessToken = _tokenService.CreateToken(_user!);
                var refreshToken = _tokenService.GenerateRefreshToken();
                SetRefreshToken(refreshToken);

                var tokenDto = new TokenDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                };

                return Ok(tokenDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Renews an access token and refresh token
        /// </summary>
        /// <param name="request">Refresh token to be verified</param>
        /// <returns>Returns TokenDto, an access token and refresh token</returns>
        /// <remarks>
        /// Sample Request:
        ///     
        ///     POST /api/tokens/renew
        ///     {
        ///         "username": "jhondoe",
        ///         "password": "jhondoe123"
        ///     }
        ///    
        /// </remarks>
        /// 
        /// <response code="200">Successfully acquired an access token</response>
        /// <response code="400">Details are invalid</response>
        /// <response code="401">User is not authorized</response>
        [HttpPost("renew")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public IActionResult RenewToken([FromBody] RenewTokenDto request)
        {
            string tokenStatus = _tokenService.Verify(request.RefreshToken, _user!);

            if (tokenStatus.Equals("Invalid"))
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (tokenStatus.Equals("Expired"))
            {
                return Unauthorized("Refresh token expired");
            }

            string accessToken = _tokenService.CreateToken(_user!);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Ok(tokenDto);
        }

        /// <summary>
        /// Sets the refresh token to the user
        /// </summary>
        /// <param name="refreshToken">Refresh token to be set</param>
        private static void SetRefreshToken(RefreshToken refreshToken)
        {
            _user!.RefreshToken = refreshToken.Token;
            _user.TokenCreated = refreshToken.Created;
            _user.TokenExpires = refreshToken.Expires;
        }
    }
}
