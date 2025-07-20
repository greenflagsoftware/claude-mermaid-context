using Xunit;
using Commerce.Shared.Models;
using Commerce.Shared.Workflows;

namespace Commerce.Unit.Tests
{
    public class UserRegistrationWorkflowTests
    {
        private readonly UserRegistrationWorkflow _workflow;

        public UserRegistrationWorkflowTests()
        {
            _workflow = new UserRegistrationWorkflow();
        }

        [Fact]
        public void Execute_WithValidInput_ReturnsSuccessMessage()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
            Assert.False(model.IsExistingUser);
        }

        [Fact]
        public void Execute_WithEmptyUsername_ReturnsValidationError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "",
                EmailAddress = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithEmptyEmail_ReturnsValidationError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "",
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Email is required", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithInvalidEmail_ReturnsValidationError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "invalid-email",
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Invalid email format", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithEmptyPassword_ReturnsValidationError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = ""
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Password is required", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithShortPassword_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "Pass1!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Password must be at least 8 characters long", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithPasswordMissingUppercase_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Password must contain uppercase, lowercase, digit, and special character", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithPasswordMissingLowercase_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "PASSWORD123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Password must contain uppercase, lowercase, digit, and special character", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithPasswordMissingDigit_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "Password!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Password must contain uppercase, lowercase, digit, and special character", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithPasswordMissingSpecialCharacter_ReturnsPasswordComplexityError()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "Password123"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Password must contain uppercase, lowercase, digit, and special character", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithMultipleValidationErrors_ReturnsAllErrors()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "",
                EmailAddress = "invalid-email",
                Password = ""
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.Contains("Invalid email format", result);
            Assert.Contains("Password is required", result);
            Assert.False(model.IsSaved);
        }

        [Theory]
        [InlineData("user@domain.com")]
        [InlineData("test.email@example.org")]
        [InlineData("user+tag@domain.co.uk")]
        public void Execute_WithValidEmails_ProcessesSuccessfully(string email)
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = email,
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
        }

        [Theory]
        [InlineData("Password123!")]
        [InlineData("MySecure@Pass1")]
        [InlineData("Complex#Password9")]
        [InlineData("Strong$123Password")]
        public void Execute_WithValidPasswords_ProcessesSuccessfully(string password)
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = password
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
        }

        [Theory]
        [InlineData("pass")]
        [InlineData("PASSWORD")]
        [InlineData("123456")]
        [InlineData("!@#$%^")]
        [InlineData("password123")]
        [InlineData("PASSWORD123")]
        [InlineData("Password!")]
        [InlineData("Password123")]
        public void Execute_WithInvalidPasswords_ReturnsPasswordError(string password)
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = password
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.True(result.Contains("Password must") || result.Contains("password"));
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_SetsModelPropertiesCorrectly()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "testuser123",
                EmailAddress = "test@example.com",
                Password = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.True(model.IsSaved);
            Assert.False(model.IsExistingUser);
            Assert.Equal("Registration successful - please check your email for confirmation", result);
        }

        [Fact]
        public void Execute_WithNullValues_HandlesGracefully()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = null!,
                EmailAddress = null!,
                Password = null!
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.Contains("Email is required", result);
            Assert.Contains("Password is required", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithWhitespaceValues_ReturnsValidationErrors()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "   ",
                EmailAddress = "   ",
                Password = "   "
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.Contains("Email is required", result);
            Assert.Contains("Password is required", result);
            Assert.False(model.IsSaved);
        }
    }
}