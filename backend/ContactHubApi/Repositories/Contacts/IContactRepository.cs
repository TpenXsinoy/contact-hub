using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Contacts
{
    public interface IContactRepository
    {
        Task<Guid> CreateContact(Contact contact);
        Task<bool> DeleteContact(Guid id);
        Task<IReadOnlyCollection<Contact>> GetAllContacts(Guid userId);
        Task<Contact?> GetContactById(Guid id);
        Task<bool> UpdateContact(Contact contact);
    }
}
