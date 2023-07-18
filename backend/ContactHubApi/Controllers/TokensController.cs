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
        private static UserTokenDto _user = new();
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

        [HttpPost("acquire")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcquireToken([FromBody] UserLoginDto userLogin)
        {
            try
            {
                _user = await _userService.GetUserByUsernameWithToken(userLogin.Username);

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

        [HttpPost("renew")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RenewToken([FromBody] RenewTokenDto request)
        {
            string tokenStatus = _tokenService.Verify(request.RefreshToken, _user);

            if (tokenStatus == "Invalid")
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (tokenStatus == "Expired")
            {
                return Unauthorized("Refresh token expired");

            }

            string accessToken = _tokenService.CreateToken(_user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            var tokenDto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token
            };

            return Ok(tokenDto);
        }

        private static void SetRefreshToken(RefreshToken newRefreshToken)
        {
            _user.RefreshToken = newRefreshToken.Token;
            _user.TokenCreated = newRefreshToken.Created;
            _user.TokenExpires = newRefreshToken.Expires;
        }
    }
}
