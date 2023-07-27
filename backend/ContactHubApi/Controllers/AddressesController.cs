using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Models;
using ContactHubApi.Services.Addresses;
using ContactHubApi.Services.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactHubApi.Controllers
{
    [Route("api/addresses")]
    [Authorize]
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

        /// <summary>
        /// Creates an address for a contact
        /// </summary>
        /// <param name="request">Address details</param>
        /// <returns>Returns the newly created address</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/addresses
        ///     {
        ///         "addressType": "Billing",
        ///         "street": "N. Bacalso Ave.",
        ///         "city": "Cebu",
        ///         "state": "Cebu",
        ///         "postalCode": "6000",
        ///         "contactId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// 
        /// </remarks>
        /// <response code="201">Successfully created an address</response>
        /// <response code="400">Address details are invalid</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Contact is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
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

        /// <summary>
        /// Gets an address
        /// </summary>
        /// <param name="id">Id of address</param>
        /// <returns>Returns the AddressDtodetails of an address</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "addressType": "Billing",
        ///         "street": "N. Bacalso Ave.",
        ///         "city": "Cebu",
        ///         "state": "Cebu",
        ///         "postalCode": "6000",
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully retrieved an address</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Address is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}", Name = "GetAddressById")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            try
            {
                var address = await _addressService.GetAddressById(id);

                if (address == null)
                {
                    return NotFound($"Address with ID {id} is not found");
                }

                return Ok(address);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Updates an address
        /// </summary>
        /// <param name="id">Id of address to be updated</param>
        /// <param name="request">Details of address to be updated</param>
        /// <returns>Returns AddressDto details of the updated contact</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///         "addressType": "Billing",
        ///         "street": "N. Bacalso Ave.",
        ///         "city": "Cebu",
        ///         "state": "Cebu",
        ///         "postalCode": "6000",
        ///         "contactId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Successfully updated an address</response>
        /// <response code="400">Address details are invalid</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Contact or address is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddressDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] AddressCreationDto request)
        {
            try
            {
                var contact = await _contactService.GetContactById(request.ContactId);

                if (contact == null)
                {
                    return NotFound($"Contact with ID {request.ContactId} is not found");
                }

                var address = await _addressService.GetAddressById(id);

                if (address == null)
                {
                    return NotFound($"Address with ID {id} is not found");
                }

                var updatedContact = await _addressService.UpdateAddress(id, request);

                return Ok(updatedContact);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, "Something went wrong");
            }
        }

        /// <summary>
        /// Deletes an address
        /// </summary>
        /// <param name="id">Id of address to be deleted</param>
        /// <returns>Returns successful message</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/addresses/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// </remarks>
        /// <response code="200">Successfully deleted an address</response>
        /// <response code="401">User is not authorized to use this endpoint</response>
        /// <response code="404">Address is not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
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
                {
                    return NotFound($"Address with ID {id} is not found");
                }

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
