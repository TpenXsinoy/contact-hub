using ContactHubApi.Dtos.Email;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Services.Email;
using ContactHubApi.Services.Tokens;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactHubApi.Controllers
{
    [Route("api/confirmation-codes")]
    [AllowAnonymous]
    [ApiController]
    public class ConfirmationCodesController : ControllerBase
    {
        private readonly ILogger<ConfirmationCodesController> _logger;
        private readonly IConfirmationCodeService _confirmationCodeService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private static EmailConfirmationCodeDto _codeConfirmationDto = new();

        public ConfirmationCodesController(
            ILogger<ConfirmationCodesController> logger,
             IConfirmationCodeService confirmationCodeService,
            IUserService userService,
            ITokenService tokenService)
        {
            _logger = logger;
            _confirmationCodeService = confirmationCodeService;
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Sends a confirmation code to the specified email
        /// </summary>
        /// <param name="email">Email where the code will be sent</param>
        /// <returns>Returns the user got by the email</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/confirmation-codes/send?email=stephine.n.sinoy%40gmail.com
        /// 
        /// </remarks>
        /// <response code="200">Successfully sent the confirmation code</response>
        /// <response code="400">Email is invalid</response>
        /// <response code="404">User is not found</response>
        /// <response code="500">Internal server error</response>.
        [HttpPost("send")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendConfirmationCode(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);

                if (user == null)
                {
                    return NotFound("User does not exist.");
                }

                _codeConfirmationDto = _confirmationCodeService.GenerateCode(email);

                _confirmationCodeService.SendConfirmationCode(new EmailDto
                {
                    To = email,
                    Body = _codeConfirmationDto.Code
                });

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Verifies the confimation code sent to a specific email
        /// </summary>
        /// <param name="request">EmailConfirmationCodeDto details</param>
        /// <returns>Returns an access token to access endpoints for updating password</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/confirmation-codes/verify
        ///     {
        ///         email = "john.doe@gmail.com",
        ///         code = "123456"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Confirmation code verified</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="500">Internal server error</response>.
        [HttpPost("verify")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyConfirmationCode([FromBody] EmailConfirmationCodeDto request)
        {
            try
            {
                var emailWithCode = _confirmationCodeService.GetConfirmationCodeWithEmail(_codeConfirmationDto);

                if (request.Email != emailWithCode.Email)
                {
                    return BadRequest("Email does not match the registered email.");
                }

                if (request.Code != emailWithCode.Code)
                {
                    return BadRequest("Invalid code");
                }

                var user = await _userService.GetUserByEmail(request.Email);

                var userTokenDto = new UserTokenDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Email = user.Email,
                };

                string accessToken = _tokenService.CreateToken(userTokenDto);

                return Ok(accessToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
