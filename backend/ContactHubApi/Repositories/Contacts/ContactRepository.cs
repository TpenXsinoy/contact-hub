using ContactHubApi.Context;
using ContactHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactHubApi.Repositories.Contacts
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactHubContext _dbContext;

        public ContactRepository(ContactHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateContact(Contact contact)
        {
            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<bool> DeleteContact(Guid id)
        {
            var contact = await _dbContext.Contacts.FindAsync(id);

            if (contact == null)
            {
                return false;
            }

            _dbContext.Addresses.RemoveRange(contact.Addresses);
            _dbContext.Contacts.Remove(contact!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyCollection<Contact>> GetAllContacts(Guid userId)
        {
            return await _dbContext.Contacts.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Contact?> GetContactById(Guid id)
        {
            return await _dbContext.Contacts.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateContact(Contact contact)
        {
            var existingContact = await _dbContext.Contacts.FindAsync(contact.Id);
            if (existingContact != null)
            {
                _dbContext.Entry(existingContact).CurrentValues.SetValues(contact);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
