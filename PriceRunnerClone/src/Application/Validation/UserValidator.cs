using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IUserValidator
    {
        IReadOnlyList<string> ValidateForCreate(string userName, string password, int? userRoleId);
        IReadOnlyList<string> ValidateForUpdate(string userName, string? password, int? userRoleId);
    }

    public sealed class UserValidator : IUserValidator
    {
        public IReadOnlyList<string> ValidateForCreate(string userName, string password, int? userRoleId)
        {
            var errors = ValidateCommon(userName, userRoleId);

            if (string.IsNullOrWhiteSpace(password))
                errors.Add("Password is required for new users.");
            else if (password.Length < 6)
                errors.Add("Password must be at least 6 characters.");

            return errors;
        }

        public IReadOnlyList<string> ValidateForUpdate(string userName, string? password, int? userRoleId)
        {
            var errors = ValidateCommon(userName, userRoleId);

            if (!string.IsNullOrWhiteSpace(password) && password.Length < 6)
                errors.Add("Password must be at least 6 characters when changed.");

            return errors;
        }

        private static List<string> ValidateCommon(string userName, int? userRoleId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(userName))
                errors.Add("UserName is required.");
            else if (userName.Length > 255)
                errors.Add("UserName must be at most 255 characters.");

            if (userRoleId.HasValue && userRoleId.Value <= 0)
                errors.Add("UserRoleId must be a positive integer when provided.");

            return errors;
        }
    }
}
