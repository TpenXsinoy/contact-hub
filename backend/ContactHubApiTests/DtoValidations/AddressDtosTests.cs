﻿using System.ComponentModel.DataAnnotations;
using ContactHubApi.Dtos.Addresses;

namespace ContactHubApiTests.Dtos
{
    public class AddressDtosTests
    {
        // AddressCreationDto Tests
        [Fact]
        public void ValidAddressCreationDto_NoValidationErrors()
        {
            // Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = "Home",
                Street = "123 Main St",
                City = "City",
                State = "State",
                PostalCode = "12345",
                ContactId = Guid.NewGuid()
            };

            var validationContext = new ValidationContext(addressCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(addressCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Theory]
        [InlineData("", "123 Main St", "City", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Address Type is required.")]
        [InlineData("Home", "123 Main St", "", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "City is required.")]
        [InlineData("Home", "123 Main St", "City", "State", "", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Postal Code is required.")]
        [InlineData("johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "123 Main St", "City", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum length for the address type is 50 characters.")]
        [InlineData("Home", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "City", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum length for the street is 100 characters.")]
        [InlineData("Home", "123 Main St", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum length for the city is 50 characters.")]
        [InlineData("Home", "123 Main St", "City", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum length for the state is 50 characters.")]
        [InlineData("Home", "123 Main St", "City", "State", "johndoe123TooLongUsernameTooLongUsernameTooLongjohndoe123TooLongUsernameTooLongUsernameTooLong", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Maximum length for the postal code is 50 characters.")]
        [InlineData("ss", "123 Main St", "City", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Minimum length for the address type is 3 characters.")]
        [InlineData("Home", "123 Main St", "d", "State", "12345", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Minimum length for the city is 2 characters.")]
        [InlineData("Home", "123 Main St", "City", "State", "s", "e9a0db04-5ef8-499b-c1ac-08db86d2cc0d", "Minimum length for the postal code is 2 characters.")]
        public void InvalidAddressCreationDto_ValidationErrors(string addressType, string street, string city,
                                                            string state, string postalCode, Guid contactId, string expectedErrorMessage)
        {
            // Arrange
            var addressCreationDto = new AddressCreationDto
            {
                AddressType = addressType,
                Street = street,
                City = city,
                State = state,
                PostalCode = postalCode,
                ContactId = contactId
            };

            var validationContext = new ValidationContext(addressCreationDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(addressCreationDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }
    }
}
