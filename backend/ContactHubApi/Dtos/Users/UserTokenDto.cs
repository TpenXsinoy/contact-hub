using System.ComponentModel.DataAnnotations;
using ContactHubApi.Models;

namespace ContactHubApi.Dtos.Users
{
    public class UserTokenDto
    {
        [Required(ErrorMessage = "Email is required.")]
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
