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

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
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

                var newContact = await _contactService.CreateContact(request);

                return CreatedAtRoute("GetContactById", new { id = newContact.Id }, newContact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet(Name = "GetAllContacts")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllContacts([FromQuery] Guid userId)
        {
            try
            {
                var user = await _userService.GetUserById(userId);

                if (user == null)
                    return NotFound($"User with ID {userId} is not found");

                var contacts = await _contactService.GetAllContacts(userId);

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

        [HttpGet("{id}", Name = "GetContactById")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(ContactAddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                    return NotFound($"Contact with ID {id} is not found");

                return Ok(contact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] ContactCreationDto request)
        {
            try
            {
                var user = await _userService.GetUserById(request.UserId);

                if (user == null)
                    return NotFound($"User with ID {request.UserId} is not found");

                var contact = await _contactService.GetContactById(id);

                if (contact == null)
                    return NotFound($"Contact with ID {id} is not found");

                var updatedContact = await _contactService.UpdateContact(id, request);

                return Ok(updatedContact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
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
                    return NotFound($"Contact with ID {id} is not found");

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
