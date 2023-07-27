using ContactHubApi.Dtos.ConfirmationCode;

namespace ContactHubApi.Services.ConfirmationCodes
{
    public interface IConfirmationCodeService
    {
        /// <summary>
        /// Generates a random 6-digit code
        /// </summary>
        /// <param name="email">Email where code will be assigned</param>
        /// <returns>EmailConfirmationCodeDto with Email and Code details</returns>
        EmailConfirmationCodeDto GenerateCode(string email);

        /// <summary>
        /// Gets the confirmation code with email
        /// </summary>
        /// <param name="request">EmailConfirmationCodeDto details</param>
        /// <returns>Email and code assigned to it</returns>
        EmailConfirmationCodeDto GetConfirmationCodeWithEmail(EmailConfirmationCodeDto request);

        /// <summary>
        /// Sends a confirmation code to the specified email
        /// </summary>
        /// <param name="request">EmailDto, to whom and what will be sent</param>
        void SendConfirmationCode(EmailDto request);
    }
}
