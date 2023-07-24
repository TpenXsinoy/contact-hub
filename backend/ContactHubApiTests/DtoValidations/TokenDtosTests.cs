using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactHubApi.Dtos.Tokens;
using ContactHubApi.Dtos.Users;

namespace ContactHubApiTests.Dtos
{
    public class TokenDtosTests
    {
        // RenewTokenDto Tests
        [Fact]
        public void ValidRenewTokenDto_NoValidationErrors()
        {
            // Arrange
            var renewTokenDto = new RenewTokenDto
            {
                RefreshToken = "refreshToken",
            };

            var validationContext = new ValidationContext(renewTokenDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(renewTokenDto, validationContext, validationResults, true);

            // Assert
            Assert.True(isValid);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void InValidRenewTokenDto_ValidationErrors()
        {
            // Arrange
            var renewTokenDto = new RenewTokenDto
            {
                RefreshToken = "",
            };

            var validationContext = new ValidationContext(renewTokenDto, null, null);
            var validationResults = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(renewTokenDto, validationContext, validationResults, true);

            // Assert
            Assert.False(isValid);
            Assert.Contains(validationResults, validationResult => validationResult.ErrorMessage == "RefreshToken is required.");
        }
    }
}
