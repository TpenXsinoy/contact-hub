using System.Security.Claims;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Services.Contacts;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ContactHubApi.Controllers
{
    [Route("api/contacts")]
    [Authorize]
    [ApiController]
    public class ContactsController : ControllerBase
    {

        private readonly ILogger<ContactsController> _logger;
        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public ContactsController(
            IContactService contactService,
            ILogger<ContactsController> logger,
            IUserService userService)
        {
            _logger = logger;
            _contactService = contactService;
            _userService = userService;
        }

        /// <summary>
        /// Creates a contact for a user
        /// </summary>
        /// <param name="request">Contact details</param>
        /// <returns>Returns the newly created contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/contacts
        ///     {
        ///         "firstName" : "John",
        ///         "lastName" : "Doe",
        ///         "userId" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully created a contact</response>
        /// <response code="400">Contact details are invalid</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">User is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateContact([FromBody] ContactCreationDto request)
        {
            try
            {
                var user = await _userService.GetUserById(request.UserId);

                if (user == null)
                {
                    return NotFound($"User with ID {request.UserId} is not found");
                }

                ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

                var loggedInUser = _userService.GetCurrentUser(identity);

                if (loggedInUser == null || user.Id != loggedInUser.Id)
                {
                    return Unauthorized();
                }


                var newContact = await _contactService.CreateContact(request);

                return CreatedAtRoute("GetContactById", new { id = newContact.Id }, newContact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Gets all contacts of a user
        /// </summary>
        /// <returns>Returns the ContactDto details of a user</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/contacts
        ///     [
        ///         {
        ///             "id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "firstName" : "John",
        ///             "lastName" : "Doe"
        ///         },
        ///         {
        ///             "id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///             "firstName" : "Jhonny",
        ///             "lastName" : "Sims"
        ///         }
        ///     ]
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved contacts of user</response>
        /// <response code="204">User has no contacts</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">User is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet(Name = "GetAllContacts")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

                var loggedInUser = _userService.GetCurrentUser(identity);

                if (loggedInUser == null)
                {
                    return Unauthorized();
                }

                var user = await _userService.GetUserByUsername(loggedInUser.Username);

                if (user == null)
                {
                    return NotFound($"User is not found");
                }

                var contacts = await _contactService.GetAllContacts(user.Id);

                if (contacts.IsNullOrEmpty())
                {
                    return NoContent();
                }

                return Ok(contacts);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Gets a contact by id
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>Returns the ContactAddressDto details of a contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/contacts/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///         "id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "firstName" : "John",
        ///         "lastName" : "Doe",
        ///         "addresses" : [
        ///             {
        ///                 "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///                 "addressType": "Billing",
        ///                 "street": "N. Bacalso Ave.",
        ///                 "city": "Cebu",
        ///                 "state": "Cebu",
        ///                 "postalCode": "6000",
        ///                 "contactId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///             }
        ///         ]   
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved contact</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Contact is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetContactById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                {
                    return NotFound($"Contact with ID {id} is not found");
                }

                ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

                var loggedInUser = _userService.GetCurrentUser(identity);

                if (loggedInUser == null || contact.UserId != loggedInUser.Id)
                {
                    return Unauthorized();
                }

                return Ok(contact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="id">Id of contact to be updated</param>
        /// <param name="request">Details of contact to be updated</param>
        /// <returns>Returns ContactDto details of the updated contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/contacts/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///         "firstName" : "John",
        ///         "lastName" : "Doe",
        ///         "userId" : "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully updated a contact</response>
        /// <response code="400">Contact details are invalid</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">User or contact is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactCreationDto request)
        {
            try
            {
                var user = await _userService.GetUserById(request.UserId);

                if (user == null)
                {
                    return NotFound($"User with ID {request.UserId} is not found");
                }

                ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

                var loggedInUser = _userService.GetCurrentUser(identity);

                if (loggedInUser == null || user.Id != loggedInUser.Id)
                {
                    return Unauthorized();
                }

                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                {
                    return NotFound($"Contact with ID {id} is not found");
                }

                var updatedContact = await _contactService.UpdateContact(id, request);

                return Ok(updatedContact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Id of contact to be deleted</param>
        /// <returns>Returns successful message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/contacts/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// </remarks>
        /// <response code="200">Successfully deleted a contact</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Contact is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                {
                    return NotFound($"Contact with ID {id} is not found");
                }

                ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

                var loggedInUser = _userService.GetCurrentUser(identity);

                if (loggedInUser == null || contact.UserId != loggedInUser.Id)
                {
                    return Unauthorized();
                }

                await _contactService.DeleteContact(id);

                return Ok($"Contact with ID {id} was successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }


    }
}
