 using System.Net;
using System.Net.Mail;
using ContactHubApi.Dtos.Email;

namespace ContactHubApi.Services.Email
{
    public class ConfirmationCodeService : IConfirmationCodeService
    {
        private readonly IConfiguration _config;
        private static readonly Random _rnd = new();
        private static readonly char[] _digits = "0123456789".ToCharArray();
        private static readonly char[] _codeBuffer = new char[6];

        public ConfirmationCodeService(IConfiguration config)
        {
            _config = config;
        }

        public EmailConfirmationCodeDto GenerateCode(string email)
        {
            for (int i = 0; i < 6; i++)
            {
                int randomDigit = _rnd.Next(0, 10);
                _codeBuffer[i] = _digits[randomDigit];
            }

            return new EmailConfirmationCodeDto
            {
                Email = email,
                Code = new string(_codeBuffer)
            };
        }

        public void SendConfirmationCode(EmailDto request)
        {
            var emailConfig = _config.GetSection("Email");
            var fromAddress = new MailAddress(emailConfig["Username"]!, "Contact Hub");
            var toAddress = new MailAddress(request.To);
            string fromPassword = emailConfig["Password"]!;
            string subject = "Password Reset Code - Contact Hub";
            // Style the body using HTML markup and inline CSS
            string body = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333;'>
                        <h1 style='color: #37bc9b;'>Password Reset Code</h1>
                        <p>Hello,</p>
                        <p>Here is your password reset code: </p>
                        <h1 style='font-size: 40px;'><strong>{request.Body}</strong></h1>
                        <p>If you didn't request this, please ignore this email.</p>
                        <p>Regards,<br />Contact Hub Team</p>
                    </body>
                </html>
            ";

            var smtp = new SmtpClient
            {
                Host = emailConfig["Host"]!,
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            smtp.Send(message);
        }

        public EmailConfirmationCodeDto GetConfirmationCodeWithEmail(EmailConfirmationCodeDto request)
        {
            return request;
        }
    }
}
