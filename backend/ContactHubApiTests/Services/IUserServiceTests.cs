using System.Security.Claims;
using AutoMapper;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Models;
using ContactHubApi.Repositories.Users;
using ContactHubApi.Services.Users;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ContactHubApiTests.Services
{
    public class IUserServiceTests
    {
        private readonly IUserService _userService;
        private readonly Mock<IUserRepository> _fakeUserRepository;
        private readonly Mock<IMapper> _fakeMapper;

        public IUserServiceTests()
        {
            _fakeUserRepository = new Mock<IUserRepository>();
            _fakeMapper = new Mock<IMapper>();
            _userService = new UserService(_fakeUserRepository.Object, _fakeMapper.Object);
        }

        // CreateUser Tests
        [Fact]
        public async Task CreateUser_UserCreated_ReturnsUser()
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

            _userService.CreatePasswordHash(userCreationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userModel = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userCreationDto.FirstName,
                LastName = userCreationDto.LastName,
                Email = userCreationDto.Email,
                Username = userCreationDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Contacts = new List<Contact>()
            };

            var userDto = new UserDto
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username
            };

            _fakeMapper.Setup(m => m.Map<User>(userCreationDto))
                        .Returns(userModel);

            _fakeUserRepository.Setup(repo => repo.CreateUser(userModel))
                                .ReturnsAsync(userModel.Id);

            _fakeMapper.Setup(m => m.Map<UserDto>(userModel))
                        .Returns(userDto);

            // Act
            var result = await _userService.CreateUser(userCreationDto);

            // Assert
            Assert.Equal(userDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
        }

        [Fact]
        public async Task CreateUser_ConnectionError_ThrowsException()
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

            var userModel = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userCreationDto.FirstName,
                LastName = userCreationDto.LastName,
                Email = userCreationDto.Email,
                Username = userCreationDto.Username,
                Contacts = new List<Contact>()
            };

            _fakeMapper.Setup(m => m.Map<User>(userCreationDto))
                        .Returns(userModel);

            _userService.CreatePasswordHash(userCreationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            _fakeUserRepository.Setup(repo => repo.CreateUser(userModel))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.CreateUser(userCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // GetUserById Tests
        [Fact]
        public async void GetUserById_HasUser_ReturnsUser()
        {
            // Arrange
            var userId = It.IsAny<Guid>();

            var userModel = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            var userUIDetailsDto = new UserUIDetailsDto
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username,
                PasswordHash = userModel.PasswordHash,
                PasswordSalt = userModel.PasswordSalt
            };

            _fakeUserRepository.Setup(repo => repo.GetUserById(userId))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserUIDetailsDto>(result);
        }

        [Fact]
        public async void GetUserById_UserNotFound_ReturnsNull()
        {
            // Arrange
            var userId = It.IsAny<Guid>();
            User? userModel = null;
            UserUIDetailsDto? userUIDetailsDto = null;

            _fakeUserRepository.Setup(repo => repo.GetUserById(userId))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.Null(result);
        }

        [Fact]
        public async void GetUserById_ConnectionError_ThrowsException()
        {
            // Arrange
            var userId = It.IsAny<Guid>();

            _fakeUserRepository.Setup(repo => repo.GetUserById(userId))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserById(userId));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // GetUserByUsername Tests
        [Fact]
        public async void GetUserByUsername_HasUser_ReturnsUser()
        {
            // Arrange
            var username = "john123";

            var userModel = new User
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = username,
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            var userUIDetailsDto = new UserUIDetailsDto
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username,
                PasswordHash = userModel.PasswordHash,
                PasswordSalt = userModel.PasswordSalt
            };

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.GetUserByUsername(username);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserUIDetailsDto>(result);
        }

        [Fact]
        public async void GetUserByUsername_UserNotFound_ReturnsNull()
        {
            // Arrange
            var username = "john123";
            User? userModel = null;
            UserUIDetailsDto? userUIDetailsDto = null;

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.GetUserByUsername(username);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.Null(result);
        }

        [Fact]
        public async void GetUserByUsername_ConnectionError_ThrowsException()
        {
            // Arrange
            var username = "john123";

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByUsername(username));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // GetUserByUsernameWithToken Tests
        [Fact]
        public async void GetUserByUsernameWithToken_HasUser_ReturnsUser()
        {
            // Arrange
            var username = "john123";

            var userModel = new User
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = username,
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            var userTokenDto = new UserTokenDto
            {
                Email = userModel.Email,
                Username = userModel.Username,
                TokenCreated = DateTime.UtcNow,
                TokenExpires = DateTime.UtcNow.AddMinutes(30),
            };

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserTokenDto>(userModel))
                        .Returns(userTokenDto);

            // Act
            var result = await _userService.GetUserByUsernameWithToken(username);

            // Assert
            Assert.Equal(userTokenDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserTokenDto>(result);
        }

        [Fact]
        public async void GetUserByUsernameWithToken_UserNotFound_ReturnsNull()
        {
            // Arrange
            var username = "john123";
            User? userModel = null;
            UserTokenDto? userTokenDto = null;

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .ReturnsAsync(userModel);

            _fakeMapper.Setup(m => m.Map<UserTokenDto>(userModel))
                        .Returns(userTokenDto);

            // Act
            var result = await _userService.GetUserByUsernameWithToken(username);

            // Assert
            Assert.Equal(userTokenDto, result);
            Assert.Null(result);
        }

        [Fact]
        public async void GetUserByUsernameWithToken_ConnectionError_ThrowsException()
        {
            // Arrange
            var username = "john123";

            _fakeUserRepository.Setup(repo => repo.GetUserByUsername(username))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.GetUserByUsernameWithToken(username));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // IsUserEmailExist Tests
        [Fact]
        public async void IsUserEmailExist_EmailExist_ReturnsTrue()
        {
            // Arrange
            var email = "john.doe@gmail.com";

            _fakeUserRepository.Setup(repo => repo.IsUserEmailExist(email))
                                .ReturnsAsync(true);

            // Act
            var result = await _userService.IsUserEmailExist(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void IsUserEmailExist_EmailDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var email = "john.doe@gmail.com";

            _fakeUserRepository.Setup(repo => repo.IsUserEmailExist(email))
                                .ReturnsAsync(false);

            // Act
            var result = await _userService.IsUserEmailExist(email);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void IsUserEmailExist_ConnectionError_ThrowsException()
        {
            // Arrange
            var email = "john.doe@gmail.com";

            _fakeUserRepository.Setup(repo => repo.IsUserEmailExist(email))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.IsUserEmailExist(email));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // IsUsernameExist Tests
        [Fact]
        public async void IsUsernameExist_UsernameExist_ReturnsTrue()
        {
            // Arrange
            var username = "john123";

            _fakeUserRepository.Setup(repo => repo.IsUsernameExist(username))
                                .ReturnsAsync(true);

            // Act
            var result = await _userService.IsUsernameExist(username);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void IsUsernameExist_UsernameDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var username = "john123";

            _fakeUserRepository.Setup(repo => repo.IsUsernameExist(username))
                                .ReturnsAsync(false);

            // Act
            var result = await _userService.IsUsernameExist(username);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void IsUsernameExist_ConnectionError_ThrowsException()
        {
            // Arrange
            var username = "john123";

            _fakeUserRepository.Setup(repo => repo.IsUsernameExist(username))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.IsUsernameExist(username));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // UpdateUser Tests
        [Fact]
        public async Task UpdateUser_UserUpdated_ReturnsUserUIDetailsDto()
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

            _userService.CreatePasswordHash(userCreationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userModel = new User
            {
                Id = userId,
                FirstName = userCreationDto.FirstName,
                LastName = userCreationDto.LastName,
                Email = userCreationDto.Email,
                Username = userCreationDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Contacts = new List<Contact>()
            };

            var userUIDetailsDto = new UserUIDetailsDto
            {
                Id = userModel.Id,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                Email = userModel.Email,
                Username = userModel.Username,
                PasswordHash = userModel.PasswordHash,
                PasswordSalt = userModel.PasswordSalt
            };

            _fakeMapper.Setup(m => m.Map<User>(userCreationDto))
                        .Returns(userModel);

            _fakeUserRepository.Setup(repo => repo.UpdateUser(userModel))
                                .ReturnsAsync(true);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.UpdateUser(userId, userCreationDto);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserUIDetailsDto>(result);
        }

        [Fact]
        public async Task UpdateUser_UserNotUpdated_ReturnsEmptyUserUIDetailsDto()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var userCreationDto = new UserCreationDto();
            var userModel = new User();
            var userUIDetailsDto = new UserUIDetailsDto();

            _fakeMapper.Setup(m => m.Map<User>(userCreationDto))
                        .Returns(userModel);

            _fakeUserRepository.Setup(repo => repo.UpdateUser(userModel))
                                .ReturnsAsync(false);

            _fakeMapper.Setup(m => m.Map<UserUIDetailsDto>(userModel))
                        .Returns(userUIDetailsDto);

            // Act
            var result = await _userService.UpdateUser(userId, userCreationDto);

            // Assert
            Assert.Equal(userUIDetailsDto, result);
            Assert.NotNull(result);
            Assert.IsType<UserUIDetailsDto>(result);
        }

        [Fact]
        public async Task UpdateUser_ConnectionError_ThrowsException()
        {
            //Arrange
            var userId = It.IsAny<Guid>();
            var userCreationDto = new UserCreationDto();
            var userModel = new User();

            _fakeMapper.Setup(m => m.Map<User>(userCreationDto))
                        .Returns(userModel);

            _fakeUserRepository.Setup(repo => repo.UpdateUser(userModel))
                                .Throws(new Exception("Database connection error"));

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => _userService.UpdateUser(userId, userCreationDto));

            // Assert
            Assert.Equal("Database connection error", result.Message);
        }

        // CreatePasswordHash Tests
        [Fact]
        public void CreatePasswordHash_Should_GenerateHashAndSalt()
        {
            // Arrange
            var password = "password";
            byte[] passwordHash;
            byte[] passwordSalt;

            // Act
            _userService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.NotEmpty(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.NotEmpty(passwordSalt);
        }

        // VerifyPasswordHash Tests
        [Fact]
        public void VerifyPasswordHash_PasswordMatches_ReturnsTrue()
        {
            // Arrange
            var password = "password";
            byte[] passwordHash;
            byte[] passwordSalt;

            _userService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Act
            var result = _userService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPasswordHash_PasswordDoesNotMatch_ReturnsFalse()
        {
            // Arrange
            var password = "password";
            var wrongPassword = "wrongPassword";
            byte[] passwordHash;
            byte[] passwordSalt;

            _userService.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            // Act
            var result = _userService.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

            // Assert
            Assert.False(result);
        }

        // GetCurrentUser Tests
        [Fact]
        public void GetCurrentUser_WithValidIdentity_ReturnsUserTokenDto()
        {
            // Arrange
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.GivenName, "John"),
                new Claim(ClaimTypes.Surname, "Doe"),
                new Claim(ClaimTypes.NameIdentifier, "johndoe123"),
                new Claim(ClaimTypes.Email, "john.doe@example.com")
            });

            // Act
            var result = _userService.GetCurrentUser(identity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            Assert.Equal("johndoe123", result.Username);
            Assert.Equal("john.doe@example.com", result.Email);
        }

        [Fact]
        public void GetCurrentUser_WithNullIdentity_ReturnsNull()
        {
            // Arrange
            ClaimsIdentity? identity = null;

            // Act
            var result = _userService.GetCurrentUser(identity);

            // Assert
            Assert.Null(result);
        }
    }
}
