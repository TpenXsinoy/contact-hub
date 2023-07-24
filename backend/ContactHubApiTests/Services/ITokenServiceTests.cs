using System;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Services.Tokens;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ContactHubApiTests.Services
{
    public class ITokenServiceTests
    {
        private readonly ITokenService _tokenService;
        private readonly Mock<IConfiguration> _fakeConfiguration;

        public ITokenServiceTests()
        {
            _fakeConfiguration = new Mock<IConfiguration>();
            _tokenService = new TokenService(_fakeConfiguration.Object);
        }
        
        // CreateToken Tests
        [Fact]
        public void CreateToken_Should_ReturnValidJwtToken()
        {
            // Arrange
            var user = new UserTokenDto
            {
                Username = "john_doe",
                Email = "john.doe@example.com"
            };

            _fakeConfiguration.Setup(config => config["Jwt:Key"])
                                .Returns("DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4");

            _fakeConfiguration.Setup(config => config["Jwt:Issuer"])
                                .Returns("http://localhost:7130/");

            _fakeConfiguration.Setup(config => config["Jwt:Audience"])
                                .Returns("http://localhost:7130/");

            // Act
            var result = _tokenService.CreateToken(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        // GenerateRefreshToken Tests
        [Fact]
        public void GenerateRefreshToken_Should_ReturnValidRefreshToken()
        {
            // Arrange
            // Act
            var result = _tokenService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
            Assert.True(result.Expires > DateTime.Now);
            Assert.True(result.Created <= DateTime.Now);
        }

        // Verify Tests
        [Theory]
        [InlineData("valid_token", "valid_token", "Valid")]
        [InlineData("invalid_token", "different_token", "Invalid")]
        [InlineData("equal_token", "equal_token", "Expired")]
        public void Verify_Should_ReturnCorrectStatus(string refreshToken, string userRefreshToken, string expectedStatus)
        {
            // Arrange
            var user = new UserTokenDto
            {
                RefreshToken = userRefreshToken,
                TokenExpires = expectedStatus == "Expired" ? DateTime.Now.AddHours(-1) : DateTime.Now.AddHours(1)
            };

            // Act
            var result = _tokenService.Verify(refreshToken, user);

            // Assert
            Assert.Equal(expectedStatus, result);
        }
    }
}
