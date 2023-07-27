using System.Net.Mail;
using ContactHubApi.Dtos.Email;
using ContactHubApi.Services.Email;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ContactHubApiTests.Services
{
    public class IConfirmationCodeServiceTests
    {
        private readonly IConfirmationCodeService _confirmationCodeService;
        private readonly Mock<IConfiguration> _fakeConfig;

        public IConfirmationCodeServiceTests()
        {
            _fakeConfig = new Mock<IConfiguration>();
            _confirmationCodeService = new ConfirmationCodeService(_fakeConfig.Object);
        }

        [Fact]
        public void GenerateCode_ShouldGenerateValidCode_ReturnsEmailWithCode()
        {
            // Arrange
            var email = "john@example.com";

            // Act
            var codeDto = _confirmationCodeService.GenerateCode(email);

            // Assert
            Assert.Equal(email, codeDto.Email);
            Assert.NotNull(codeDto.Code);
            Assert.True(codeDto.Code.Length == 6, "Code length should be 6");
        }

        [Fact]
        public void SendConfirmationCode_ShouldNotThrowException()
        {
            // Arrange
            var emailDto = new EmailDto
            {
                To = "recipient@example.com",
                Body = "123456"
            };

            // Mock IConfiguration
            _fakeConfig.Setup(config => config.GetSection("Email")["Host"]).Returns("smtp.gmail.com");
            _fakeConfig.Setup(config => config.GetSection("Email")["Username"]).Returns("ssinoy@fullscale.io");
            _fakeConfig.Setup(config => config.GetSection("Email")["Password"]).Returns("hidpfbhkfpltjzid");

            // Act & Assert
            try
            {
                _confirmationCodeService.SendConfirmationCode(emailDto);
                // If the method does not throw an exception, the test will pass
            }
            catch (Exception ex)
            {
                // If the method throws an exception, the test will fail
                Assert.True(false, $"An unexpected exception was thrown: {ex.Message}");
            }
        }

        [Fact]
        public void GetConfirmationCodeWithEmail_ShouldReturnSameObject()
        {
            // Arrange
            var emailDto = new EmailConfirmationCodeDto
            {
                Email = "john@example.com",
                Code = "123456"
            };

            // Act
            var result = _confirmationCodeService.GetConfirmationCodeWithEmail(emailDto);

            // Assert
            Assert.Same(emailDto, result);
        }
    }
}
