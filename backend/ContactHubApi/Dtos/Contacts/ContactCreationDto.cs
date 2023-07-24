using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Contacts
{
    public class ContactCreationDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the first name is 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum lenghth for the last name is 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
    }
}
