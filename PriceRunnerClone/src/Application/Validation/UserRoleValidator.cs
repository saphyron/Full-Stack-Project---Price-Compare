using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IUserRoleValidator
    {
        IReadOnlyList<string> ValidateForCreate(string name);
        IReadOnlyList<string> ValidateForUpdate(string name);
    }

    public sealed class UserRoleValidator : IUserRoleValidator
    {
        public IReadOnlyList<string> ValidateForCreate(string name)
            => ValidateName(name);

        public IReadOnlyList<string> ValidateForUpdate(string name)
            => ValidateName(name);

        private static List<string> ValidateName(string name)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
                errors.Add("Name is required.");
            else if (name.Length > 255)
                errors.Add("Name must be at most 255 characters.");

            return errors;
        }
    }
}
