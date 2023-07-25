using ContactHubApi.Controllers;
using ContactHubApi.Dtos.Tokens;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Services.Tokens;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactHubApiTests.Controllers
{
    public class TokensControllerTests
    {
        private readonly TokensController _controller;
        private readonly Mock<ITokenService> _fakeTokenService;
        private readonly Mock<ILogger<TokensController>> _fakeLogger;
        private readonly Mock<IUserService> _fakeUserService;

        public TokensControllerTests()
        {
            _fakeTokenService = new Mock<ITokenService>();
            _fakeLogger = new Mock<ILogger<TokensController>>();
            _fakeUserService = new Mock<IUserService>();
            _controller = new TokensController(
                _fakeLogger.Object,
                _fakeTokenService.Object,
                _fakeUserService.Object);
        }

        // AcquireToken Tests
        [Fact]
        public async Task AcquireToken_TokenAcquired_ReturnsOk()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            var user = new UserTokenDto();

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                            .ReturnsAsync(new UserUIDetailsDto()
                            {
                                Id = It.IsAny<Guid>(),
                                FirstName = "test",
                                LastName = "test",
                                Username = "test",
                                Email = "test@gmail.com"
                            });

            _fakeUserService.Setup(service => service.VerifyPasswordHash(userLoginDto.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                             .Returns(true);


            _fakeTokenService.Setup(service => service.CreateToken(user))
                            .Returns("token");

            _fakeTokenService.Setup(service => service.GenerateRefreshToken())
                            .Returns(new RefreshToken());

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task AcquireToken_UserDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                           .ReturnsAsync((UserUIDetailsDto?) null);

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task AcquireToken_InvalidPassword_ReturnsBadRequest()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = It.IsAny<Guid>(),
                               FirstName = "test",
                               LastName = "test",
                               Username = "test",
                               Email = "test@gmail.com"
                           });

            _fakeUserService.Setup(service => service.VerifyPasswordHash(userLoginDto.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                             .Returns(false);

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task AcquireToken_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task AcquireToken_CreateTokenError_ReturnsInternalServerError()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            var user = new UserTokenDto
            {
                Username = "john_doe",
                Email = "john.doe@example.com"
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = It.IsAny<Guid>(),
                               FirstName = "test",
                               LastName = "test",
                               Username = "test",
                               Email = "test@gmail.com"
                           });

            _fakeUserService.Setup(service => service.VerifyPasswordHash(userLoginDto.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                             .Returns(true);

            _fakeTokenService.Setup(service => service.CreateToken(user))
                            .Throws(new Exception());

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task AcquireToken_GenerateRefreshTokenError_ReturnsInternalServerError()
        {
            //Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "john123",
                Password = "password",
            };

            var user = new UserTokenDto
            {
                Username = "john_doe",
                Email = "john.doe@example.com"
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(userLoginDto.Username))
                           .ReturnsAsync(new UserUIDetailsDto()
                           {
                               Id = It.IsAny<Guid>(),
                               FirstName = "test",
                               LastName = "test",
                               Username = "test",
                               Email = "test@gmail.com"
                           });

            _fakeUserService.Setup(service => service.VerifyPasswordHash(userLoginDto.Password, It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                             .Returns(true);

            _fakeTokenService.Setup(service => service.CreateToken(user))
                            .Returns("token");

            _fakeTokenService.Setup(service => service.GenerateRefreshToken())
                            .Throws(new Exception());

            // Act
            var result = await _controller.AcquireToken(userLoginDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // RenewToken Tests
        [Fact]
        public void RenewToken_TokenRenewed_ReturnsOk()
        {
            //Arrange
            var renewTokenDto = new RenewTokenDto
            {
                RefreshToken = "refreshToken",
            };

            var user = new UserTokenDto
            {
                Username = "john_doe",
                Email = "john.doe@example.com"
            };

            _fakeTokenService.Setup(service => service.Verify(renewTokenDto.RefreshToken, It.IsAny<UserTokenDto>()))
                    .Returns("Valid")
                    .Callback((string refreshToken, UserTokenDto userDto) =>
                    {
                        userDto.Username = user.Username;
                        userDto.Email = user.Email;
                    });

            _fakeTokenService.Setup(service => service.CreateToken(user))
                            .Returns("token");

            _fakeTokenService.Setup(service => service.GenerateRefreshToken())
                            .Returns(new RefreshToken());

            // Act
            var result = _controller.RenewToken(renewTokenDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Theory]
        [InlineData("Invalid")]
        [InlineData("Expired")]
        public void RenewToken_TokenInvalid_ReturnsUnaithorized(string tokenStatus)
        {
            //Arrange
            var renewTokenDto = new RenewTokenDto
            {
                RefreshToken = "refreshToken",
            };

            var user = new UserTokenDto
            {
                Username = "john_doe",
                Email = "john.doe@example.com"
            };

            _fakeTokenService.Setup(service => service.Verify(renewTokenDto.RefreshToken, It.IsAny<UserTokenDto>()))
                    .Returns(tokenStatus)
                    .Callback((string refreshToken, UserTokenDto userDto) =>
                    {
                        userDto.Username = user.Username;
                        userDto.Email = user.Email;
                    });

            // Act
            var result = _controller.RenewToken(renewTokenDto);

            //Assert
            var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        }
    }
}
