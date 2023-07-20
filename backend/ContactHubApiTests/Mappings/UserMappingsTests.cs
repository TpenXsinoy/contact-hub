using AutoMapper;
using ContactHubApi.Dtos.Users;
using ContactHubApi.Mappings;
using ContactHubApi.Models;
using Moq;

namespace ContactHubApiTests.Mappings
{
    public class UserMappingsTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public UserMappingsTests()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile<UserMappings>());
            _mapper = _configuration.CreateMapper();
        }

        // UserCreationDto to User
        [Fact]
        public void Map_ValidUserCreationDto_ReturnsUser()
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

            var user = new User
            {
                Id = It.IsAny<Guid>(),
                FirstName = userCreationDto.FirstName,
                LastName = userCreationDto.LastName,
                Email = userCreationDto.Email,
                Username = userCreationDto.Username,
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            //Act
            var result = _mapper.Map<User>(userCreationDto);

            //Assert
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.PasswordHash, result.PasswordHash);
            Assert.Equal(user.PasswordSalt, result.PasswordSalt);
            Assert.Equal(user.Contacts, result.Contacts);
        }

        // User to UserDto
        [Fact]
        public void Map_ValidUser_ReturnsUserDto()
        {
            //Arrange
            var user = new User
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username
            };

            //Act
            var result = _mapper.Map<UserDto>(user);

            //Assert
            Assert.Equal(userDto.Id, result.Id);
            Assert.Equal(userDto.FirstName, result.FirstName);
            Assert.Equal(userDto.LastName, result.LastName);
            Assert.Equal(userDto.Email, result.Email);
            Assert.Equal(userDto.Username, result.Username);
        }

        // User to UserUIDetailsDto
        [Fact]
        public void Map_ValidUser_ReturnsUserUIDetailsDto()
        {
            //Arrange
            var user = new User
            {
                Id = It.IsAny<Guid>(),
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
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt
            };

            //Act
            var result = _mapper.Map<UserUIDetailsDto>(user);

            //Assert
            Assert.Equal(userUIDetailsDto.Id, result.Id);
            Assert.Equal(userUIDetailsDto.FirstName, result.FirstName);
            Assert.Equal(userUIDetailsDto.LastName, result.LastName);
            Assert.Equal(userUIDetailsDto.Email, result.Email);
            Assert.Equal(userUIDetailsDto.Username, result.Username);
            Assert.Equal(userUIDetailsDto.PasswordHash, result.PasswordHash);
            Assert.Equal(userUIDetailsDto.PasswordSalt, result.PasswordSalt);
        }

        // User to UserTokenDto
        [Fact]
        public void Map_ValidUser_ReturnsUserTokenDto()
        {
            //Arrange
            var user = new User
            {
                Id = It.IsAny<Guid>(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                Username = "john123",
                PasswordHash = Array.Empty<byte>(),
                PasswordSalt = Array.Empty<byte>(),
                Contacts = new List<Contact>()
            };

            var userTokenDto = new UserTokenDto
            {
                Email = user.Email,
                Username = user.Username,
                RefreshToken = "",
                TokenCreated = DateTime.UtcNow,
                TokenExpires = DateTime.UtcNow.AddMinutes(30)
            };

            //Act
            var result = _mapper.Map<UserTokenDto>(user);

            //Assert
            Assert.Equal(userTokenDto.Email, result.Email);
            Assert.Equal(userTokenDto.Username, result.Username);
            Assert.Equal(userTokenDto.RefreshToken, result.RefreshToken);
        }
    }
}
