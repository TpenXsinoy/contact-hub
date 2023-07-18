using ContactHubApi.Dtos.Contacts;
using ContactHubApi.Models;

namespace ContactHubApi.Services.Contacts
{
    public interface IContactService
    {
        /// <summary>
        /// Creates a contact
        /// </summary>
        /// <param name="contact">ContactCreationDto details</param>
        /// <returns>Contact details of the newly created contact</returns>
        Task<Contact> CreateContact(ContactCreationDto contact);

        /// <summary>
        /// Deletes a contact
        /// </summary>
        /// <param name="id">Id of contact to be deleted</param>
        /// <returns>True if deletion is successful, otherwise false</returns>
        Task<bool> DeleteContact(Guid id);

        /// <summary>
        /// Gets all contacts of a user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>ContactDto details of a user</returns>
        Task<IReadOnlyCollection<ContactDto>> GetAllContacts(Guid userId);

        /// <summary>
        /// Gets a contact by id
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>ContactAddressDto details with id the same as the param</returns>
        Task<ContactAddressDto?> GetContactById(Guid id);

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="id">Id of contact to be updated</param>
        /// <param name="contact">Details of contact to be updated</param>
        /// <returns>ContactDto details of the updated contact</returns>
        Task<ContactDto> UpdateContact(Guid id, ContactCreationDto contact);
    }
}
