using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Contacts
{
    public class ContactRepository : IContactRepository
    {
        public Task<Guid> CreateContact(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteContact(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Contact>> GetAllContacts()
        {
            throw new NotImplementedException();
        }

        public Task<Contact?> GetContactById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateContact(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
