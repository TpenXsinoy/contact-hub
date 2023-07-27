using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.ConfirmationCode
{
    public class EmailDto
    {
        [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Email is invalid.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body is required.")]
        public string Body { get; set; } = string.Empty;
    }
}
