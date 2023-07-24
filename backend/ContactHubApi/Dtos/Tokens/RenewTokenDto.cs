using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Dtos.Tokens
{
    public class RenewTokenDto
    {
        [Required(ErrorMessage = "RefreshToken is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
