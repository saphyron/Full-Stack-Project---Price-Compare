using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IShopValidator
    {
        IReadOnlyList<string> ValidateForCreate(string fullName, string? shopUrl, int brandId, int categoryId);
        IReadOnlyList<string> ValidateForUpdate(string fullName, string? shopUrl, int brandId, int categoryId);
    }

    public sealed class ShopValidator : IShopValidator
    {
        public IReadOnlyList<string> ValidateForCreate(string fullName, string? shopUrl, int brandId, int categoryId)
            => ValidateCommon(fullName, shopUrl, brandId, categoryId);

        public IReadOnlyList<string> ValidateForUpdate(string fullName, string? shopUrl, int brandId, int categoryId)
            => ValidateCommon(fullName, shopUrl, brandId, categoryId);

        private static List<string> ValidateCommon(string fullName, string? shopUrl, int brandId, int categoryId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add("FullName is required.");
            else if (fullName.Length > 255)
                errors.Add("FullName must be at most 255 characters.");

            if (!string.IsNullOrWhiteSpace(shopUrl) && shopUrl.Length > 2000)
                errors.Add("ShopUrl must be at most 2000 characters.");

            if (brandId <= 0)
                errors.Add("BrandId must be a positive integer.");

            if (categoryId <= 0)
                errors.Add("CategoryId must be a positive integer.");

            return errors;
        }
    }
}
