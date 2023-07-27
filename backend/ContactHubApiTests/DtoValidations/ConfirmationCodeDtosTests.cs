using System.ComponentModel.DataAnnotations;
using ContactHubApi.Dtos.Email;

namespace ContactHubApiTests.DtoValidations
{
    public class ConfirmationCodeDtosTests
    {
        // EmailConfirmationCodeDto Tests
        [Fact]
        public void ValidEmailConfirmationCodeDto_NoValidationErrors()
        {
            // Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = "john@gmail.com",
                Code = "123456"
            };

            var validationContext = new ValidationContext(emailConfirmationCodeDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(emailConfirmationCodeDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "123456", "Email is required.")]
        [InlineData("john@gmail.com", "", "Code is required.")]
        [InlineData("johngmail.com", "123456", "Email is invalid.")]
        public void InvalidEmailConfirmationCodeDto_ValidationErrors(string email, string code, string expectedErrorMessage)
        {
            // Arrange
            var emailConfirmationCodeDto = new EmailConfirmationCodeDto
            {
                Email = email,
                Code = code
            };

            var validationContext = new ValidationContext(emailConfirmationCodeDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(emailConfirmationCodeDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }

        // EmailDto Tests
        [Fact]
        public void ValidEmailDto_NoValidationErrors()
        {
            // Arrange
            var emailDto = new EmailDto
            {
                To = "john@gmail.com",
                Body = "123456"
            };

            var validationContext = new ValidationContext(emailDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(emailDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "test", "Email is required.")]
        [InlineData("john@gmail.com", "", "Body is required.")]
        [InlineData("johngmail.com", "test", "Email is invalid.")]
        public void InvalidEmailDto_ValidationErrors(string to, string body, string expectedErrorMessage)
        {
            // Arrange
            var emailDto = new EmailDto
            {
                To = to,
                Body = body
            };

            var validationContext = new ValidationContext(emailDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(emailDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }
    }
}
