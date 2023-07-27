using ContactHubApi.Controllers;
using ContactHubApi.Dtos.Email;
using ContactHubApi.Dtos.Tokens;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Services.Email;
using ContactHubApi.Services.Tokens;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactHubApiTests.Controllers
{
    public class ConfirmationCodesControllerTests
    {
        private readonly ConfirmationCodesController _controller;
        private readonly Mock<ILogger<ConfirmationCodesController>> _fakeLogger;
        private readonly Mock<IConfirmationCodeService> _fakeConfirmationCodeService;
        private readonly Mock<ITokenService> _fakeTokenService;
        private readonly Mock<IUserService> _fakeUserService;

        public ConfirmationCodesControllerTests()
        {
            _fakeLogger = new Mock<ILogger<ConfirmationCodesController>>();
            _fakeConfirmationCodeService = new Mock<IConfirmationCodeService>();
            _fakeUserService = new Mock<IUserService>();
            _fakeTokenService = new Mock<ITokenService>();
            _controller = new ConfirmationCodesController(
                _fakeLogger.Object,
                _fakeConfirmationCodeService.Object,
                _fakeUserService.Object,
                _fakeTokenService.Object);
        }

        // SendConfirmationCode Tests
        [Fact]
        public async Task SendConfirmationCode_ConfirmationCodeSent_ReturnsOk()
        {
            //Arrange
            var email = "john.doe@gmail.com";
            var code = "123456";

            var user = new UserUIDetailsDto
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                Username = "john123",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
            };

            var codeConfirmationDto = new EmailConfirmationCodeDto
            {
                Email = email,
                Code = code
            };

            var emailDto = new EmailDto
            {
                To = email,
                Body = codeConfirmationDto.Code
            };

            _fakeUserService.Setup(service => service.GetUserByEmail(email))
                            .ReturnsAsync(user);

            _fakeConfirmationCodeService.Setup(service => service.GenerateCode(email))
                            .Returns(codeConfirmationDto);

            _fakeConfirmationCodeService.Setup(service => service.SendConfirmationCode(emailDto));


            // Act
            var result = await _controller.SendConfirmationCode(email);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task SendConfirmationCode_UserDoesNotExist_ReturnsNotFound()
        {
            //Arrange
            var email = "john.doe@gmail.com";

            _fakeUserService.Setup(service => service.GetUserByEmail(email))
                            .ReturnsAsync((UserUIDetailsDto?) null);

            // Act
            var result = await _controller.SendConfirmationCode(email);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task SendConfirmationCode_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var email = "john.doe@gmail.com";

            _fakeUserService.Setup(service => service.GetUserByEmail(email))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.SendConfirmationCode(email);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // VerifyConfirmationCode Tests
        [Fact]
        public async Task VerifyConfirmationCode_ConfirmationCodeVerified_ReturnsOk()
        {
            //Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = "john@gmail.com",
                Code = "123456"
            };

            var user = new UserUIDetailsDto
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = emailConfirmationCodeDto.Email,
                Username = "john123",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
            };

            var userTokenDto = new UserTokenDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
            };

            _fakeConfirmationCodeService.Setup(service => service.GetConfirmationCodeWithEmail(It.IsAny<EmailConfirmationCodeDto>()))
                                  .Returns(emailConfirmationCodeDto);

            _fakeUserService.Setup(service => service.GetUserByEmail(emailConfirmationCodeDto.Email))
                          .ReturnsAsync(user);

            _fakeTokenService.Setup(service => service.CreateToken(userTokenDto))
                            .Returns("token");

            // Act
            var result = await _controller.VerifyConfirmationCode(emailConfirmationCodeDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task VerifyConfirmationCode_EmailDoesNotMatch_ReturnsBadRequest()
        {
            //Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = "john@gmail.com",
                Code = "123456"
            };

            _fakeConfirmationCodeService.Setup(service => service.GetConfirmationCodeWithEmail(It.IsAny<EmailConfirmationCodeDto>()))
                                  .Returns(new EmailConfirmationCodeDto
                                  {
                                      Email = "test@gmail.com",
                                      Code = emailConfirmationCodeDto.Code
                                  });

            // Act
            var result = await _controller.VerifyConfirmationCode(emailConfirmationCodeDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task VerifyConfirmationCode_CodeInvalid_ReturnsBadRequest()
        {
            //Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = "john@gmail.com",
                Code = "123456"
            };

            _fakeConfirmationCodeService.Setup(service => service.GetConfirmationCodeWithEmail(It.IsAny<EmailConfirmationCodeDto>()))
                                  .Returns(new EmailConfirmationCodeDto
                                  {
                                      Email = emailConfirmationCodeDto.Email,
                                      Code = "12345"
                                  });

            // Act
            var result = await _controller.VerifyConfirmationCode(emailConfirmationCodeDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task VerifyConfirmationCode_ServerError_ReturnsInternalServerError()
        { 
            //Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = "john@gmail.com",
                Code = "123456"
            };

            _fakeConfirmationCodeService.Setup(service => service.GetConfirmationCodeWithEmail(It.IsAny<EmailConfirmationCodeDto>()))
                                  .Returns(emailConfirmationCodeDto);

            _fakeUserService.Setup(service => service.GetUserByEmail(emailConfirmationCodeDto.Email))
                         .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.VerifyConfirmationCode(emailConfirmationCodeDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}
