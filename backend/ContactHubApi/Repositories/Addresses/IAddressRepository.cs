using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Addresses
{
    public interface IAddressRepository
    {
        /// <summary>
        /// Creates an address
        /// </summary>
        /// <param name="address">Address details</param>
        /// <returns>Id of the newly created address</returns>
        Task<Guid> CreateAddress(Address address);

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
        /// <returns>Address with id the same as the param</returns>
        Task<Address?> GetAddressById(Guid id);

        /// <summary>
        /// Updates an address
        /// </summary>
        /// <param name="address">Details of address to be updated</param>
        /// <returns>True if update is successful, otherwise false</returns>
        Task<bool> UpdateAddress(Address address);
    }
}
