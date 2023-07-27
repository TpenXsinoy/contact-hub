using System.ComponentModel.DataAnnotations;
using ContactHubApi.Utils;

namespace ContactHubApi.Dtos.Users
{
    public class UserCreationDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the first name is 50 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the first name is 2 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the last name is 50 characters.")]
        [MinLength(2, ErrorMessage = "Minimum length for the last name is 2 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is invalid.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the email is 50 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the username is 50 characters.")]
        [MinLength(5, ErrorMessage = "Minimum length for the username is 5 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(50, ErrorMessage = "Maximum length for the password is 50 characters.")]
        [StrongPassword(ErrorMessage = "The password field must have at least 8 characters, at least 1 numeric, at least 1 lowercase, at least 1 uppercase, and at least 1 special character.")]
        public string Password { get; set; } = string.Empty;
    }
}
