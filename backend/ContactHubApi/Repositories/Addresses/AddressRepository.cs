using ContactHubApi.Context;
using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Addresses
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ContactHubContext _dbContext;

        public AddressRepository(ContactHubContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<Guid> CreateAddress(Address address)
        {
            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync();
            return address.Id;
        }

        public async Task<bool> DeleteAddress(Guid id)
        {
            var address = await _dbContext.Addresses.FindAsync(id);
            _dbContext.Addresses.Remove(address!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Address?> GetAddressById(Guid id)
        {
            return await _dbContext.Addresses.FindAsync(id);
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            var existingAddress = await _dbContext.Addresses.FindAsync(address.Id);
            if (existingAddress != null)
            {
                _dbContext.Entry(existingAddress).CurrentValues.SetValues(address);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
