using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Contacts
{
    public interface IContactRepository
    {
        Task<Guid> CreateContact(User user);
        Task<Contact?> GetContactById(Guid id);
        Task<IReadOnlyCollection<Contact>> GetAllContacts();
        Task<bool> UpdateContact(Contact contact);
        Task<bool> DeleteContact(Guid id);
    }
}
