using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Users
{
    public class UserTokenDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "RefreshToken is required.")]
        public string RefreshToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "TokenCreated is required.")]
        public DateTime TokenCreated { get; set; }

        [Required(ErrorMessage = "TokenExpires is required.")]
        public DateTime TokenExpires { get; set; }
    }
}
