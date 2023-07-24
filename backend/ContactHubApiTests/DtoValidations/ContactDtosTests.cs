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
        [InlineData("", "doe", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "First Name is required.")]
        [InlineData("johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "doe", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum lenghth for the first name is 50 characters.")]
        [InlineData("john", "", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Last Name is required.")]
        [InlineData("johndoe123", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum lenghth for the last name is 50 characters.")]
        public void InvalidContactCreationDto_ValidationErrors(string firstName, string lastName, Guid userId, string expectedErrorMessage)
        {
            // Arrange
            var contactCreationDto = new ContactCreationDto
            {
                FirstName = firstName,
                LastName = lastName,
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
