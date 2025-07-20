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
        public void Execute_WithValidInput_ProcessesWorkflowSuccessfully()
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
            var result = _workflow.Execute(model);

            // Assert
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
            Assert.False(model.IsExistingUser);
        }

        [Fact]
        public void Execute_WithValidationErrors_ReturnsValidationErrorsFromModel()
        {
            // Arrange - Invalid model with multiple validation issues
            var model = new UserRegistrationModel
            {
                Username = "",
                EmailAddress = "invalid-email",
                EmailAddressConfirmation = "different-email",
                Password = "weak",
                PasswordConfirmation = "different"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.Contains("Invalid email format", result);
            Assert.False(model.IsSaved);
        }

        [Fact]
        public void Execute_WithValidationFailure_DoesNotProcessWorkflowSteps()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "", // Invalid - will cause validation failure
                EmailAddress = "test@example.com",
                EmailAddressConfirmation = "test@example.com",
                Password = "Password123!",
                PasswordConfirmation = "Password123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert
            Assert.Contains("Username is required", result);
            Assert.False(model.IsSaved); // Workflow steps not executed
            Assert.False(model.IsExistingUser); // Workflow steps not executed
        }

        [Fact]
        public void Execute_SetsModelPropertiesCorrectlyAfterSuccessfulWorkflow()
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
            var result = _workflow.Execute(model);

            // Assert - Verify workflow completed successfully and set expected properties
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
            Assert.False(model.IsExistingUser);
        }

        [Fact]
        public void Execute_CallsModelValidation_BeforeProcessingWorkflow()
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
            var result = _workflow.Execute(model);

            // Assert - Verify that validation was called (properties set by validation)
            Assert.True(model.IsEmailValid);
            Assert.True(model.IsPasswordValid);
            Assert.Empty(model.ValidationErrors);
        }

        [Fact]
        public void Execute_WithValidModel_ExecutesCompleteWorkflowProcess()
        {
            // Arrange
            var model = new UserRegistrationModel
            {
                Username = "uniqueuser456",
                EmailAddress = "unique@example.com",
                EmailAddressConfirmation = "unique@example.com",
                Password = "SecurePass123!",
                PasswordConfirmation = "SecurePass123!"
            };

            // Act
            var result = _workflow.Execute(model);

            // Assert - Verify entire workflow executed
            Assert.Equal("Registration successful - please check your email for confirmation", result);
            Assert.True(model.IsSaved);
            Assert.False(model.IsExistingUser);
            Assert.True(model.IsEmailValid);
            Assert.True(model.IsPasswordValid);
        }
    }
}