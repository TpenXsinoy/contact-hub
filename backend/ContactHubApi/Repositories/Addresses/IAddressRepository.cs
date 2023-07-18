using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Addresses
{
    public interface IAddressRepository
    {
        Task<Guid> CreateAddress(Address address);
        Task<bool> DeleteAddress(Guid id);
        Task<Address?> GetAddressById(Guid id);
        Task<bool> UpdateAddress(Address address);
    }
}
