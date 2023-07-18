using ContactHubApi.Dtos.Addresses;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Addresses
{
    public interface IAddressService
    {
        Task<Address> CreateAddress(AddressCreationDto address);
        Task<bool> DeleteAddress(Guid id);
        Task<AddressDto?> GetAddressById(Guid id);
        Task<AddressDto> UpdateAddress(Guid id, AddressCreationDto address);
    }
}
