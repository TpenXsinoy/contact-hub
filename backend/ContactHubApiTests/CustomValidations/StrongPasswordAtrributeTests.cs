using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactHubApi.Utils;

namespace ContactHubApiTests.Utils
{
    public class StrongPasswordAtrributeTests
    {
        [Theory]
        [InlineData("Strongpwd123!", true)] // Valid password with all criteria met
        [InlineData("Weakpassword!", false)] // Invalid password, missing numeric character
        [InlineData("weakpassword123!", false)] // Invalid password, missing uppercase letter
        [InlineData("WEAKPASSWORD123!", false)] // Invalid password, missing lowercase letter
        [InlineData("WEAKPASSWORD123", false)] // Invalid password, missing special character
        [InlineData("Weak1P!", false)] // Invalid password, less than 8 characters
        [InlineData("", false)] // Invalid password, empty string
        [InlineData(null, false)] // Invalid password, null value
        public void IsValid_ValidatesPassword(string password, bool expectedResult)
        {
            // Arrange
            var attribute = new StrongPasswordAttribute();

            // Act
            bool isValid = attribute.IsValid(password);

            // Assert
            Assert.Equal(expectedResult, isValid);
        }
    }
}
