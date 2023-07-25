using System.ComponentModel.DataAnnotations;
using ContactHubApi.Dtos.Contacts;
using Moq;

namespace ContactHubApiTests.Dtos
{
    public class ContactDtosTests
    {
        // ContactCreationDto Tests
        [Fact]
        public void ValidContactCreationDto_NoValidationErrors()
        {
            // Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = "john",
                LastName = "doe",
                PhoneNumber = "1234567890",
                UserId = It.IsAny<Guid>()
            };

            var validationContext = new ValidationContext(contactCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(contactCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "doe", "1234567890", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "First name is required.")]
        [InlineData("johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "doe", "1234567890", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum lenghth for the first name is 50 characters.")]
        [InlineData("john", "", "1234567890", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Last name is required.")]
        [InlineData("johndoe123", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "1234567890", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum lenghth for the last name is 50 characters.")]
        [InlineData("john", "doe", "", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Phone number is required.")]
        public void InvalidContactCreationDto_ValidationErrors(string firstName, string lastName, string phoneNumber, Guid userId, string expectedErrorMessage)
        {
            // Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                UserId = userId
            };

            var validationContext = new ValidationContext(contactCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(contactCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }
    }
}
