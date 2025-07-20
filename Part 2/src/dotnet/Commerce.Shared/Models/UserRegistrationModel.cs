using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Commerce.Shared.Models
{
    public class UserRegistrationModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string EmailAddressConfirmation { get; set; } = string.Empty;
        public bool IsSaved { get; set; } = false;
        public bool IsEmailValid { get; set; } = false;
        public bool IsPasswordValid { get; set; } = false;
        public bool IsExistingUser { get; set; } = false;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public List<string> ValidateFields()
        {
            var errors = new List<string>();

            // Validate Username
            if (string.IsNullOrWhiteSpace(Username))
            {
                errors.Add("Username is required");
            }
            else if (Username.Length < 3)
            {
                errors.Add("Username must be at least 3 characters long");
            }

            // Validate Email
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                errors.Add("Email is required");
            }
            else
            {
                IsEmailValid = IsValidEmailFormat(EmailAddress);
                if (!IsEmailValid)
                {
                    errors.Add("Invalid email format");
                }
            }

            // Validate Email Confirmation
            if (string.IsNullOrWhiteSpace(EmailAddressConfirmation))
            {
                errors.Add("Email address confirmation is required");
            }
            else if (EmailAddress != EmailAddressConfirmation)
            {
                errors.Add("Email addresses do not match");
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(Password))
            {
                errors.Add("Password is required");
            }
            else
            {
                var passwordResult = CheckPasswordComplexity(Password);
                IsPasswordValid = passwordResult.IsValid;
                if (!IsPasswordValid)
                {
                    errors.Add(passwordResult.Message);
                }
            }

            // Validate Password Confirmation
            if (string.IsNullOrWhiteSpace(PasswordConfirmation))
            {
                errors.Add("Password confirmation is required");
            }
            else if (Password != PasswordConfirmation)
            {
                errors.Add("Passwords do not match");
            }

            ValidationErrors = errors;
            return errors;
        }

        public PasswordResult CheckPasswordComplexity(string password)
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

        private bool ValidatePasswordComplexity(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUppercase = Regex.IsMatch(password, @"[A-Z]");
            bool hasLowercase = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]");

            return hasUppercase && hasLowercase && hasDigit && hasSpecialChar;
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
}