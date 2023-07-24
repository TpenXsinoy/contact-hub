using ContactHubApi.Models;

namespace ContactHubApi.Repositories.Contacts
{
    public interface IContactRepository
    {
        /// <summary>
        /// Creates a contact
        /// </summary>
        /// <param name="contact">Contact details</param>
        /// <returns>Id of the newly created contact</returns>
        Task<Guid> CreateContact(Contact contact);

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
        /// <returns>Contacts of a user with the specified Id</returns>
        Task<IReadOnlyCollection<Contact>> GetAllContacts(Guid userId);

        /// <summary>
        /// Gets a contact by id
        /// </summary>
        /// <param name="id">Contact Id</param>
        /// <returns>Contact with id the same as the param</returns>
        Task<Contact?> GetContactById(Guid id);

        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="contact">Details of contact to be updated</param>
        /// <returns>True if update is successful, otherwise false</returns>
        Task<bool> UpdateContact(Contact contact);
    }
}
