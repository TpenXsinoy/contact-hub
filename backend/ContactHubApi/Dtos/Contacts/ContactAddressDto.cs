using ContactHubApi.Models;

namespace ContactHubApi.Dtos.Contacts
{
    public class ContactAddressDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
