using System.ComponentModel.DataAnnotations;
using ContactHubApi.Dtos.Users;

namespace ContactHubApiTests.Dtos
{
    public class UserDtosTests
    {
        // UserCreationDto Tests
        [Fact]
        public void ValidUserCreationDto_NoValidationErrors()
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe123",
                Password = "Password123!"
            };

            var validationContext = new ValidationContext(userCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "Doe", "john.doe@example.com", "johndoe123", "password123", "First Name is required.")]
        [InlineData("John", "", "john.doe@example.com", "johndoe123", "password123", "Last Name is required.")]
        [InlineData("John", "Doe", "notanemail", "johndoe123", "password123", "Email is invalid.")]
        [InlineData("John", "Doe", "john.doe@example.com", "", "password123", "Username is required.")]
        [InlineData("John", "Doe", "john.doe@example.com", "johndoe123", "", "Password is required.")]
        [InlineData("JohnTooLongFirstNameTooLongFirstNameTooLongFirstNameTooLong", "Doe", "john.doe@example.com", "johndoe123", "password123", "Maximum lenghth for the first name is 50 characters.")]
        [InlineData("John", "TooLongLastNameTooLongLastNameTooLongLastNameTooLongLastName", "john.doe@example.com", "johndoe123", "password123", "Maximum lenghth for the last name is 50 characters.")]
        [InlineData("John", "Doe", "john.doe@example.com", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "password123", "Maximum lenghth for the username is 50 characters.")]
        [InlineData("John", "Doe", "john.doe@example.com", "johndoe123", "password123TooLongPasswordTooLongPasswordTooLongpassword123TooLongPasswordTooLongPasswordTooLong", "Maximum lenghth for the password is 50 characters.")]
        [InlineData("John", "Doe", "john.doe@example.com", "johndoe123", "weak", "The password field must have at least 8 characters, at least 1 numeric, at least 1 lowercase, at least 1 uppercase, and at least 1 special character.")]
        [InlineData("L", "Doe", "john.doe@example.com", "johndoe123", "password123", "Minimum lenghth for the first name is 2 characters.")]
        [InlineData("John", "D", "john.doe@example.com", "johndoe123", "password123", "Minimum lenghth for the last name is 2 characters.")]
        [InlineData("John", "Doe", "john.doe@example.com", "user", "password123", "Minimum lenghth for the username is 5 characters.")]
        public void InvalidUserCreationDto_ValidationErrors(string firstName, string lastName, string email,
                                                            string username, string password, string expectedErrorMessage)
        {
            // Arrange
            var userCreationDto = new UserCreationDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Username = username,
                Password = password
            };

            var validationContext = new ValidationContext(userCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }

        // UserLoginDto Tests
        [Fact]
        public void ValidUserLoginDto_NoValidationErrors()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = "johndoe123",
                Password = "password123"
            };

            var validationContext = new ValidationContext(userLoginDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "password", "Username is required.")]
        [InlineData("johndoe123", "", "Password is required.")]
        public void InvalidUserLoginDto_ValidationErrors(string username, string password, string expectedErrorMessage)
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Username = username,
                Password = password
            };

            var validationContext = new ValidationContext(userLoginDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }

        // UserTokenDto Tests
        [Fact]
        public void ValidUserTokenDto_NoValidationErrors()
        {
            // Arrange
            var userTokenDto = new UserTokenDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Username = "johndoe123",
                RefreshToken = "refreshToken",
                TokenCreated = DateTime.Now,
                TokenExpires = DateTime.Now,
            };

            var validationContext = new ValidationContext(userTokenDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userTokenDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "test", "john.doe@example.com", "john_doe", "refreshToken123", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "First Name is required.")]
        [InlineData("test", "", "john.doe@example.com", "john_doe", "refreshToken123", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "Last Name is required.")]
        [InlineData("test", "test", "", "john_doe", "refreshToken123", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "Email is required.")]
        [InlineData("test", "test", "johndoeexmaple.com", "john_doe", "refreshToken123", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "Email is invalid.")]
        [InlineData("test", "test", "john.doe@example.com", "", "refreshToken123", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "Username is required.")]
        [InlineData("test", "test", "john.doe@example.com", "john_doe", "", "2023-07-20T12:00:00", "2023-07-21T12:00:00", "RefreshToken is required.")]
        public void InvalidUserTokenDto_ValidationErrors(string firstName, string lastName, string email, string username, string refreshToken,
                                                        string tokenCreated, string tokenExpires, string expectedErrorMessage)
        {
            // Arrange
            var userTokenDto = new UserTokenDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Username = username,
                RefreshToken = refreshToken,
                TokenCreated = DateTime.Parse(tokenCreated),
                TokenExpires = DateTime.Parse(tokenExpires)

            };

            var validationContext = new ValidationContext(userTokenDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(userTokenDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }
    }
}
