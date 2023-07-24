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

        /// <summary>
        /// Log in to ContactHub
        /// </summary>
        /// <param name="request">The user to login</param>
        /// <returns>User details</returns>
        /// <remarks>
        /// Sample Request:
        ///     
        ///     POST /api/users/login
        ///     {
        ///         "username": "jhondoe",
        ///         "password": "jhondoe123"
        ///     }
        ///    
        /// </remarks>
        /// 
        /// <response code="200">Successfully logged in an account</response>
        /// <response code="400">Details are invalid</response>
        /// <response code="404">Username is not found</response>
        /// <response code="500">Internal server error</response>
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
                {
                    return NotFound($"User {request.Username} is not found");
                }

                if (!_userService.VerifyPasswordHash(request.Password!, user.PasswordHash, user.PasswordSalt))
                {
                    return BadRequest("Wrong password!");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="request">User details</param>
        /// <returns>Returns the newly created UserDto details</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/users/signup
        ///     {
        ///         "firstName" : "John",
        ///         "lastName" : "Doe",
        ///         "email" : "john.doe@gmail.com",
        ///         "username" : "john123",
        ///         "password" : "John123!"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully created a user</response>
        /// <response code="400">User details are invalid</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("signup")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreationDto request)
        {
            try
            {
                var isUsernameExist = await _userService.IsUsernameExist(request.Username!);

                if (isUsernameExist)
                {
                    return BadRequest($"Username {request.Username} is already taken");
                }

                var isEmailExist = await _userService.IsUserEmailExist(request.Email!);

                if (isEmailExist)
                {
                    return BadRequest($"Email {request.Email} is already taken");
                }

                var newUser = await _userService.CreateUser(request);

                return CreatedAtRoute("GetUserById", new { id = newUser.Id }, newUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Returns UserUIDetailsDto details</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/users/e9a0db04-5ef8-499b-c1ac-08db86d2cc0d
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "email": "john.doe@gmail.com",
        ///         "username": "john123",
        ///         "passwordHash": "0xFB180C5CD89DFB99182CCEA4A",
        ///         "passwordSalt": "0xFB180C5CD89DFB99182CCEA4A"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved user</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">User with the given id is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
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
                {
                    return NotFound($"User with ID {id} is not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets a user by username
        /// </summary>
        /// <param name="username">Username of user</param>
        /// <returns>Returns UserUIDetailsDto details</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/users/e9a0db04-5ef8-499b-c1ac-08db86d2cc0d
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "email": "john.doe@gmail.com",
        ///         "username": "john123",
        ///         "passwordHash": "0xFB180C5CD89DFB99182CCEA4A",
        ///         "passwordSalt": "0xFB180C5CD89DFB99182CCEA4A"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved user</response>
        /// <response code="404">User with the given username is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{username}/details", Name = "GetUserByUsername")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserUIDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsername(username);

                if (user == null)
                {
                    return NotFound($"User {username} is not found");
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <param name="request">UserCreationDto details that will be updated</param>
        /// <returns>Returns the updated UserUIDetailsDto details</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/users/e9a0db04-5ef8-499b-c1ac-08db86d2cc0d
        ///     {
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "email": "john.doe@gmail.com",
        ///         "username": "john123",
        ///         "password": "123"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved user</response>
        /// <response code="400">UserCreationDto details are invalid</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">User with the given id is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Authorize]
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
                {
                    return NotFound($"User with ID {id} is not found");
                }

                var isUsernameExist = await _userService.IsUsernameExist(request.Username!);

                if (user.Username != request.Username && isUsernameExist)
                {
                    return BadRequest($"Username {request.Username} is already taken");
                }

                var isEmailExist = await _userService.IsUserEmailExist(request.Email!);

                if (user.Email != request.Email && isEmailExist)
                {
                    return BadRequest($"Email {request.Email} is already taken");
                }

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
