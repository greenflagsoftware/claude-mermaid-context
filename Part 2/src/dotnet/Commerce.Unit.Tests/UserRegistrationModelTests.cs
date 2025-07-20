using Xunit;
using Commerce.Shared.Models;

namespace Commerce.Unit.Tests
{
    public class UserRegistrationModelTests
    {
        [Fact]
        public void ValidateFields_WithValidInput_ReturnsNoErrors()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Empty(errors);
            Assert.True(model.IsEmailValid);
            Assert.True(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithEmptyUsername_ReturnsUsernameRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Username is required", errors);
            Assert.Contains("Username is required", model.ValidationErrors);
        }

        [Fact]
        public void ValidateFields_WithShortUsername_ReturnsUsernameMinLengthError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "ab",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Username must be at least 3 characters long", errors);
        }

        [Fact]
        public void ValidateFields_WithWhitespaceUsername_ReturnsUsernameRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "   ",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Username is required", errors);
        }

        [Fact]
        public void ValidateFields_WithEmptyEmail_ReturnsEmailRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "",
                EmailAddressConfirmation = "",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Email is required", errors);
            Assert.False(model.IsEmailValid);
        }

        [Fact]
        public void ValidateFields_WithInvalidEmailFormat_ReturnsInvalidEmailError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "invalid-email",
                EmailAddressConfirmation = "invalid-email",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Invalid email format", errors);
            Assert.False(model.IsEmailValid);
        }

        [Theory]
        [InlineData("user@domain.com")]
        [InlineData("test.email@example.org")]
        [InlineData("user+tag@domain.co.uk")]
        [InlineData("firstname.lastname@company.com")]
        public void ValidateFields_WithValidEmails_SetsEmailValidToTrue(string email)
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = email,
                EmailAddressConfirmation = email,
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Empty(errors);
            Assert.True(model.IsEmailValid);
        }

        [Fact]
        public void ValidateFields_WithMissingEmailConfirmation_ReturnsEmailConfirmationRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Email address confirmation is required", errors);
        }

        [Fact]
        public void ValidateFields_WithMismatchedEmailConfirmation_ReturnsEmailMismatchError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "different@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Email addresses do not match", errors);
        }

        [Fact]
        public void ValidateFields_WithEmptyPassword_ReturnsPasswordRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "",
                PasswordConfirmation = ""
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password is required", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithShortPassword_ReturnsPasswordLengthError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Pass1!",
                PasswordConfirmation = "Pass1!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password must be at least 8 characters long", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithPasswordMissingUppercase_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "password123!",
                PasswordConfirmation = "password123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password must contain uppercase, lowercase, digit, and special character", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithPasswordMissingLowercase_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "PASSWORD123!",
                PasswordConfirmation = "PASSWORD123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password must contain uppercase, lowercase, digit, and special character", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithPasswordMissingDigit_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password!",
                PasswordConfirmation = "Password!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password must contain uppercase, lowercase, digit, and special character", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithPasswordMissingSpecialCharacter_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123",
                PasswordConfirmation = "Password123"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password must contain uppercase, lowercase, digit, and special character", errors);
            Assert.False(model.IsPasswordValid);
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("MySecure@Pass1")]
        [InlineData("Complex#Password9")]
        [InlineData("Strong$123Password")]
        public void ValidateFields_WithValidPasswords_SetsPasswordValidToTrue(string password)
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = password,
                PasswordConfirmation = password
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Empty(errors);
            Assert.True(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithMissingPasswordConfirmation_ReturnsPasswordConfirmationRequiredError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = ""
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Password confirmation is required", errors);
        }

        [Fact]
        public void ValidateFields_WithMismatchedPasswordConfirmation_ReturnsPasswordMismatchError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "DifferentPassword123!"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Passwords do not match", errors);
        }

        [Fact]
        public void ValidateFields_WithMultipleErrors_ReturnsAllErrors()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "",
                EmailAddress = "invalid-email",
                EmailAddressConfirmation = "different-email",
                Password = "weak",
                PasswordConfirmation = "different"
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Username is required", errors);
            Assert.Contains("Invalid email format", errors);
            Assert.Contains("Email addresses do not match", errors);
            Assert.Contains("Password must be at least 8 characters long", errors);
            Assert.Contains("Passwords do not match", errors);
            Assert.False(model.IsEmailValid);
            Assert.False(model.IsPasswordValid);
        }

        [Fact]
        public void ValidateFields_WithNullValues_HandlesGracefully()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = null!,
                EmailAddress = null!,
                EmailAddressConfirmation = null!,
                Password = null!,
                PasswordConfirmation = null!
            };

            // Act
            var errors = model.ValidateFields();

            // Assert
            Assert.Contains("Username is required", errors);
            Assert.Contains("Email is required", errors);
            Assert.Contains("Email address confirmation is required", errors);
            Assert.Contains("Password is required", errors);
            Assert.Contains("Password confirmation is required", errors);
        }

        [Fact]
        public void CheckPasswordComplexity_WithValidPassword_ReturnsValidResult()
        {
            // Arrange
            var model = new UserRegistrationModel();
            var password = "Password123!";

            // Act
            var result = model.CheckPasswordComplexity(password);

            // Assert
            Assert.True(result.IsValid);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public void CheckPasswordComplexity_WithShortPassword_ReturnsInvalidWithMessage()
        {
            // Arrange
            var model = new UserRegistrationModel();
            var password = "Pass1!";

            // Act
            var result = model.CheckPasswordComplexity(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Password must be at least 8 characters long", result.Message);
        }

        [Fact]
        public void CheckPasswordComplexity_WithWeakPassword_ReturnsInvalidWithMessage()
        {
            // Arrange
            var model = new UserRegistrationModel();
            var password = "password123"; // missing uppercase and special character

            // Act
            var result = model.CheckPasswordComplexity(password);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Password must contain uppercase, lowercase, digit, and special character", result.Message);
        }
    }
}