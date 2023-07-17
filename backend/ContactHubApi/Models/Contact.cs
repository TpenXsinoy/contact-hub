namespace ContactHubApi.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
