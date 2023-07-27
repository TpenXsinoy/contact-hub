using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Contacts
{
    public class ContactCreationDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the first name is 50 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the first name is 2 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the last name is 50 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the last name is 2 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the phone number is 15 characters.")]
        [MinLength(5, ErrorMessage = "Minimum length for the phone number is 5 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
    }
}
