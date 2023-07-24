using ContactHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactHubApi.Context
{
    public class ContactHubContext : DbContext
    {
        /// <summary>
        /// Sets up the database context for the application using Entity Framework Core
        /// </summary>
        public ContactHubContext(DbContextOptions<ContactHubContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
