using System.ComponentModel.DataAnnotations;

namespace ContactHubApi.Utils
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is string password))
            {
                return false;
            }

            // Password must have at least 8 characters
            if (password.Length < 8)
            {
                return false;
            }

            // Password must have at least 1 numeric character
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // Password must have at least 1 lowercase character
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // Password must have at least 1 uppercase character
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // Password must have at least 1 special character (non-alphanumeric)
            if (!password.Any(c => !char.IsLetterOrDigit(c)))
            {
                return false;
            }

            return true;
        }
    }
}
