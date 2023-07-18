using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Addresses
{
    public interface IAddressService
    {
        /// <summary>
        /// Creates an address
        /// </summary>
        /// <param name="address">AddressCreationDto details</param>
        /// <returns>Address details of the newly created address</returns>
        Task<Address> CreateAddress(AddressCreationDto address);

        /// <summary>
        /// Deletes an address
        /// </summary>
        /// <param name="id">Id of address to be deleted</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        Task<bool> DeleteAddress(Guid id);

        /// <summary>
        /// Gets an address by id
        /// </summary>
        /// <param name="id">Address Id</param>
        /// <returns>AddressDto details with id the same as the param</returns>
        Task<AddressDto?> GetAddressById(Guid id);

        /// <summary>
        /// Updates an address
        /// </summary>
        /// <param name="id">Id of the address to be updated</param>
        /// <param name="address">Details of address to be updated</param>
        /// <returns>AddressDto details of the updated address</returns>
        Task<AddressDto> UpdateAddress(Guid id, AddressCreationDto address);
    }
}
