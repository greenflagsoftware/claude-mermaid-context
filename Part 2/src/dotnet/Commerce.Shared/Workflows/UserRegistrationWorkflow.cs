using Commerce.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Commerce.Shared.Workflows
{
    public class UserRegistrationWorkflow
    {
        public string Execute(UserRegistrationModel model)
        {
            var validationResult = ValidateUserInput(model.Username, model.EmailAddress, model.Password);
            if (!validationResult.IsValid)
            {
                LogError("Input validation failed", string.Join(", ", validationResult.Errors));
                return string.Join(", ", validationResult.Errors);
            }

            var passwordComplexityResult = CheckPasswordComplexity(model.Password);
            if (!passwordComplexityResult.IsValid)
            {
                LogError("Password complexity check failed", passwordComplexityResult.Message);
                return passwordComplexityResult.Message;
            }

            bool userExists = CheckUserExists(model.Username);
            if (userExists)
            {
                model.IsExistingUser = true;
                LogError("User already exists", model.Username);
                return "Username already taken";
            }

            var saveResult = SaveUserRegistration(model.Username, model.EmailAddress, model.Password);
            if (!saveResult.IsSuccess)
            {
                LogError("Failed to save user registration", saveResult.Error ?? "Unknown error");
                return "Registration failed - please try again";
            }

            model.IsSaved = true;
            string confirmationCode = GenerateConfirmationCode();
            var emailResult = SendConfirmationEmail(model.EmailAddress, confirmationCode);
            if (!emailResult.IsSuccess)
            {
                LogWarning("Failed to send confirmation email", model.EmailAddress);
            }

            LogEvent("User registration successful", model.Username);
            return "Registration successful - please check your email for confirmation";
        }

        private ValidationResult ValidateUserInput(string username, string email, string password)
        {
            var errors = new List<string>();
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username is required");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email is required");
                isValid = false;
            }
            else
            {
                bool emailValid = IsValidEmailFormat(email);
                if (!emailValid)
                {
                    errors.Add("Invalid email format");
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password is required");
                isValid = false;
            }

            return new ValidationResult(isValid, errors);
        }

        private PasswordResult CheckPasswordComplexity(string password)
        {
            bool isValid = true;
            string message = "";

            if (password.Length < 8)
            {
                isValid = false;
                message = "Password must be at least 8 characters long";
                return new PasswordResult(isValid, message);
            }

            bool hasUppercase = ContainsUppercase(password);
            bool hasLowercase = ContainsLowercase(password);
            bool hasDigit = ContainsDigit(password);
            bool hasSpecialChar = ContainsSpecialCharacter(password);

            if (!hasUppercase || !hasLowercase || !hasDigit || !hasSpecialChar)
            {
                isValid = false;
                message = "Password must contain uppercase, lowercase, digit, and special character";
            }

            return new PasswordResult(isValid, message);
        }

        private bool CheckUserExists(string username)
        {
            // Simulated database query - in real implementation this would connect to database
            // DatabaseQuery("SELECT COUNT(*) FROM Users WHERE Username = ?", username)
            // For now, return false to simulate user doesn't exist
            return false;
        }

        private SaveResult SaveUserRegistration(string username, string email, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);
                // Simulated database insert - in real implementation this would save to database
                // DatabaseInsert("Users", userRecord)
                
                return new SaveResult(true, null);
            }
            catch (Exception ex)
            {
                return new SaveResult(false, ex.Message);
            }
        }

        private EmailResult SendConfirmationEmail(string email, string confirmationCode)
        {
            try
            {
                string emailSubject = "Please confirm your email address";
                string emailBody = BuildConfirmationEmailBody(confirmationCode);
                
                // Simulated email service call
                // EmailService.Send(email, emailSubject, emailBody)
                
                // Simulated database update for confirmation code
                // DatabaseUpdate("Users", "ConfirmationCode", confirmationCode, "Email", email)
                
                return new EmailResult(true, null);
            }
            catch (Exception ex)
            {
                return new EmailResult(false, ex.Message);
            }
        }

        private string GenerateConfirmationCode()
        {
            int codeLength = 6;
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string confirmationCode = "";
            
            Random random = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                int randomIndex = random.Next(0, characters.Length);
                confirmationCode += characters[randomIndex];
            }

            return confirmationCode;
        }

        private bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        private bool ContainsUppercase(string password)
        {
            return Regex.IsMatch(password, @"[A-Z]");
        }

        private bool ContainsLowercase(string password)
        {
            return Regex.IsMatch(password, @"[a-z]");
        }

        private bool ContainsDigit(string password)
        {
            return Regex.IsMatch(password, @"\d");
        }

        private bool ContainsSpecialCharacter(string password)
        {
            return Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private string BuildConfirmationEmailBody(string confirmationCode)
        {
            return $@"
                <html>
                <body>
                    <h2>Welcome! Please confirm your email address</h2>
                    <p>Thank you for registering. Please use the following confirmation code:</p>
                    <h3>{confirmationCode}</h3>
                    <p>If you did not register for this account, please ignore this email.</p>
                </body>
                </html>";
        }

        private void LogError(string message, string details)
        {
            DateTime timestamp = DateTime.Now;
            string logEntry = $"[ERROR] {timestamp} - {message}: {details}";
            WriteToLogFile(logEntry);
        }

        private void LogWarning(string message, string details)
        {
            DateTime timestamp = DateTime.Now;
            string logEntry = $"[WARNING] {timestamp} - {message}: {details}";
            WriteToLogFile(logEntry);
        }

        private void LogEvent(string message, string details)
        {
            DateTime timestamp = DateTime.Now;
            string logEntry = $"[INFO] {timestamp} - {message}: {details}";
            WriteToLogFile(logEntry);
        }

        private void WriteToLogFile(string logEntry)
        {
            // Simulated log file writing - in real implementation this would write to a log file
            Console.WriteLine(logEntry);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public ValidationResult(bool isValid, List<string> errors)
        {
            IsValid = isValid;
            Errors = errors ?? new List<string>();
        }
    }

    public class PasswordResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public PasswordResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message ?? "";
        }
    }

    public class SaveResult
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }

        public SaveResult(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }

    public class EmailResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public EmailResult(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }
    }
}