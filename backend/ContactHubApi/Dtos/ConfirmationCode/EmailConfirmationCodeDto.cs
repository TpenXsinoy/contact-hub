using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Email
{
    public class EmailConfirmationCodeDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required.")]
        public string Code { get; set; } = string.Empty;
    }
}
