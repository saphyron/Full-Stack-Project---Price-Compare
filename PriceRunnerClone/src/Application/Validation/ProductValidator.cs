using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IProductValidator
    {
        IReadOnlyList<string> ValidateForCreate(string name, string? productUrl, int? shopId, int brandId, int categoryId);
        IReadOnlyList<string> ValidateForUpdate(string name, string? productUrl, int? shopId, int brandId, int categoryId);
    }

    /// <summary>
    /// Simple validation rules for product input.
    /// No external dependencies, pure logic.
    /// </summary>
    public sealed class ProductValidator : IProductValidator
    {
        public IReadOnlyList<string> ValidateForCreate(
            string name,
            string? productUrl,
            int? shopId,
            int brandId,
            int categoryId)
            => ValidateCommon(name, productUrl, shopId, brandId, categoryId);

        public IReadOnlyList<string> ValidateForUpdate(
            string name,
            string? productUrl,
            int? shopId,
            int brandId,
            int categoryId)
            => ValidateCommon(name, productUrl, shopId, brandId, categoryId);

        private static List<string> ValidateCommon(
            string name,
            string? productUrl,
            int? shopId,
            int brandId,
            int categoryId)
        {
            var errors = new List<string>();

            // Name
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Name is required.");
            }
            else if (name.Length > 255)
            {
                errors.Add("Name must be at most 255 characters.");
            }

            // ProductUrl (optional, but length-limited)
            if (!string.IsNullOrWhiteSpace(productUrl) && productUrl!.Length > 2000)
            {
                errors.Add("ProductUrl must be at most 2000 characters.");
            }

            // ShopId (optional, but if set must be > 0)
            if (shopId.HasValue && shopId.Value <= 0)
            {
                errors.Add("ShopId must be a positive integer when provided.");
            }

            // BrandId (required, NOT NULL in DB)
            if (brandId <= 0)
            {
                errors.Add("BrandId must be a positive integer.");
            }

            // CategoryId (required, NOT NULL in DB)
            if (categoryId <= 0)
            {
                errors.Add("CategoryId must be a positive integer.");
            }

            return errors;
        }
    }
}
