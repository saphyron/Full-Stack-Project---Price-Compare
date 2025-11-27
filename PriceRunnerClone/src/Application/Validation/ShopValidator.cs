using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IShopValidator
    {
        IReadOnlyList<string> ValidateForCreate(string fullName, string? shopUrl);
        IReadOnlyList<string> ValidateForUpdate(string fullName, string? shopUrl);
    }


    public sealed class ShopValidator : IShopValidator
    {
        public IReadOnlyList<string> ValidateForCreate(string fullName, string? shopUrl)
            => ValidateCommon(fullName, shopUrl);

        public IReadOnlyList<string> ValidateForUpdate(string fullName, string? shopUrl)
            => ValidateCommon(fullName, shopUrl);

        private static List<string> ValidateCommon(string fullName, string? shopUrl)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add("FullName is required.");

            if (fullName?.Length > 255)
                errors.Add("FullName must be at most 255 characters.");

            if (shopUrl is not null && shopUrl.Length > 2000)
                errors.Add("ShopUrl is too long.");

            return errors;
        }
    }

}
