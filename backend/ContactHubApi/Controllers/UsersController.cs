using ContactHubApi.Dtos.Users;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactHubApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUIDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            try
            {
                var user = await _userService.GetUserByUsername(request.Username!);

                if (user == null)
                    return NotFound($"User {request.Username} is not found");

                if (!_userService.VerifyPasswordHash(request.Password!, user.PasswordHash, user.PasswordSalt))
                    return BadRequest("Wrong password!");

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto request)
        {
            try
            {
                var isUsernameExist = await _userService.IsUsernameExist(request.Username!);

                if (isUsernameExist)
                    return BadRequest($"Username {request.Username} is already taken");

                var isEmailExist = await _userService.IsUserEmailExist(request.Email!);

                if (isEmailExist)
                    return BadRequest($"Email {request.Email} is already taken");

                var newUser = await _userService.CreateUser(request);

                return CreatedAtRoute("GetUserById", new { id = newUser.Id }, newUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("{id}", Name = "GetUserById")]
        [AllowAnonymous]
        //[Authorize]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUIDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user == null)
                    return NotFound($"User with ID {id} is not found");

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("{username}/details", Name = "GetUserByUsername")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUIDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTakerByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsername(username);

                if (user == null)
                    return NotFound($"User {username} is not found");

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        //[Authorize]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUIDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreationDto request)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user == null)
                    return NotFound($"User with ID {id} is not found");

                var isUsernameExist = await _userService.IsUsernameExist(request.Username!);

                if (user.Username != request.Username && isUsernameExist)
                    return BadRequest($"Username {request.Username} is already taken");

                var isEmailExist = await _userService.IsUserEmailExist(request.Email!);

                if (user.Email != request.Email && isEmailExist)
                    return BadRequest($"Email {request.Email} is already taken");

                var updatedUser = await _userService.UpdateUser(id, request);

                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
