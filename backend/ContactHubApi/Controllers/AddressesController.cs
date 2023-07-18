using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;
using ContactHubApi.Services.Addresses;
using ContactHubApi.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactHubApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly ILogger<AddressesController> _logger;
        private readonly IAddressService _addressService;
        private readonly IContactService _contactService;

        public AddressesController(
            IAddressService addressService,
            ILogger<AddressesController> logger,
            IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
            _addressService = addressService;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(Address), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAddress([FromBody] AddressCreationDto request)
        {
            try
            {
                var contact = await _contactService.GetContactById(request.ContactId);

                if (contact == null)
                {
                    return NotFound($"Contact with ID {request.ContactId} is not found");
                }

                var newAddress = await _addressService.CreateAddress(request);

                return CreatedAtRoute("GetAddressById", new { id = newAddress.Id }, newAddress);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        [HttpGet("{id}", Name = "GetAddressById")]
        [Produces("application/json")]
        [AllowAnonymous]
        //[Authorize]
        [ProducesResponseType(typeof(ContactAddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressById(id);

                if (address == null)
                    return NotFound($"Address with ID {id} is not found");

                return Ok(address);
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
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] AddressCreationDto request)
        {
            try
            {
                var contact = await _contactService.GetContactById(request.ContactId);

                if (contact == null)
                    return NotFound($"Contact with ID {request.ContactId} is not found");

                var address = await _addressService.GetAddressById(id);

                if (address == null)
                    return NotFound($"Address with ID {id} is not found");

                var updatedContact = await _addressService.UpdateAddress(id, request);

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
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressById(id);

                if (address == null)
                    return NotFound($"Address with ID {id} is not found");

                await _addressService.DeleteAddress(id);

                return Ok($"Address with ID {id} was successfully deleted");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
