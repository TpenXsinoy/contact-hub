using ContactHubApi.Controllers;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactHubApiTests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _fakeUserService;
        private readonly Mock<ILogger<UsersController>> _fakeLogger;

        public UsersControllerTests()
        {
            _fakeUserService = new Mock<IUserService>();
            _fakeLogger = new Mock<ILogger<UsersController>>();
            _controller = new UsersController(_fakeLogger.Object, _fakeUserService.Object);
        }

        // Login Tests
        [Fact]
        public async Task Login_UserLoggedIn_ReturnsOk()
        {
            // Arrange 
            var user = new UserLoginDto
            {
                Username = "john123",
                Password = "123"
            };

            var fakeUser = new UserUIDetailsDto
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = user.Username,
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(user.Username))
                            .ReturnsAsync(fakeUser);

            _fakeUserService.Setup(service => service.VerifyPasswordHash(user.Password, fakeUser.PasswordHash, fakeUser.PasswordSalt))
                            .Returns(true);

            // Act
            var result = await _controller.Login(user);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Login_WrongPassword_ReturnsBadRequest()
        {
            // Arrange 
            var user = new UserLoginDto
            {
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(user.Username))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.VerifyPasswordHash(user.Password, Array.Empty<byte>(), Array.Empty<byte>()))
                             .Returns(false);

            // Act
            var result = await _controller.Login(user);

            // Assert
            var badRequestObjectResult =  Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsNotFound()
        {
            // Arrange 
            var user = new UserLoginDto
            {
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(user.Username))
                            .ReturnsAsync((UserUIDetailsDto?)null);

            // Act
            var result = await _controller.Login(user);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task Login_ServerError_ReturnsInternalServerError()
        {
            // Arrange 
            var user = new UserLoginDto
            {
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserByUsername(user.Username))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Login(user);

            // Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // RegisterUser Tests
        [Fact]
        public async Task RegisterUser_UserRegistered_ReturnsCreated()
        {
            //Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.CreateUser(userCreationDto))
                               .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.RegisterUser(userCreationDto);

            //Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, createdAtRouteResult.StatusCode);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task RegisterUser_UsernameOrEmailExists_ReturnsBadRequest(bool isUsernameExist, bool isEmailExist)
        {
            //Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(isUsernameExist);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(isEmailExist);

            // Act
            var result = await _controller.RegisterUser(userCreationDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_IsUsernameExistServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.RegisterUser(userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_IsUserEmailExistServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.RegisterUser(userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_CreateServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.CreateUser(userCreationDto))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.RegisterUser(userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // GetUserById Tests
        [Fact]
        public async Task GetUserById_UserExists_ReturnsOk()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            _fakeUserService.Setup(service => service.GetUserById(userId))
                             .ReturnsAsync(new UserUIDetailsDto());

            //Act
            var result = await _controller.GetUserById(userId);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_UserDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            _fakeUserService.Setup(service => service.GetUserById(userId))
                             .ReturnsAsync((UserUIDetailsDto?) null);

            //Act
            var result = await _controller.GetUserById(userId);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            _fakeUserService.Setup(service => service.GetUserById(userId))
                             .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetUserById(userId);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // GetUserByUsername Tests
        [Fact]
        public async Task GetUserByUsername_UserExists_ReturnsOk()
        {
            //Arrange
            var username = It.IsAny<string>();

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                             .ReturnsAsync(new UserUIDetailsDto());

            //Act
            var result = await _controller.GetUserByUsername(username);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUserByUsername_UserDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var username = It.IsAny<string>();

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                             .ReturnsAsync((UserUIDetailsDto?)null);

            //Act
            var result = await _controller.GetUserByUsername(username);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Fact]
        public async Task GetUserByUsername_ServerError_ReturnsInternalServerError()
        {
            //Arrange
            var username = It.IsAny<string>();

            _fakeUserService.Setup(service => service.GetUserByUsername(username))
                             .ThrowsAsync(new Exception());

            //Act
            var result = await _controller.GetUserByUsername(username);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        // UpdateUser Tests
        [Fact]
        public async Task UpdateUser_UserUpdated_ReturnsOk()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.UpdateUser(userId, userCreationDto))
                               .ReturnsAsync(new UserUIDetailsDto());

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_UserDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync((UserUIDetailsDto?) null);

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task UpdateUser_UsernameOrEmailExists_ReturnsBadRequest(bool isUsernameExist, bool isEmailExist)
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(isUsernameExist);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(isEmailExist);

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_GetUserByIdServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };
            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_IsUsernameExistServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };
            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateUser(userId,userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_IsUserEmailExistServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }

        [Fact]
        public async Task UpdateUser_UpdateServerError_ReturnsInternalServerError()
        {
            //Arrange
            var userId = It.IsAny<Guid>();

            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                Password = "123"
            };

            _fakeUserService.Setup(service => service.GetUserById(userId))
                            .ReturnsAsync(new UserUIDetailsDto());

            _fakeUserService.Setup(service => service.IsUsernameExist(userCreationDto.Username))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.IsUserEmailExist(userCreationDto.Email))
                            .ReturnsAsync(false);

            _fakeUserService.Setup(service => service.UpdateUser(userId, userCreationDto))
                            .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.UpdateUser(userId, userCreationDto);

            //Assert
            var serverError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverError.StatusCode);
        }
    }
}
