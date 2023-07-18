using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Contacts
{
    public interface IContactService
    {
        Task<Contact> CreateContact(ContactCreationDto contact);
        Task<bool> DeleteContact(Guid id);
        Task<IReadOnlyCollection<ContactDto>> GetAllContacts(Guid userId);
        Task<ContactAddressDto?> GetContactById(Guid id);
        Task<ContactDto> UpdateContact(Guid id, ContactCreationDto contact);
    }
}
