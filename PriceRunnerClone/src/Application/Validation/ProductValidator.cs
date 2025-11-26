using System.Collections.Generic;

namespace PriceRunner.Application.Validation
{
    public interface IProductValidator
    {
        IReadOnlyList<string> ValidateForCreate(string name, string? productUrl, int? shopId);
        IReadOnlyList<string> ValidateForUpdate(string name, string? productUrl, int? shopId);
    }

    /// <summary>
    /// Simple validation rules for product input.
    /// No external dependencies, pure logic.
    /// </summary>
    public sealed class ProductValidator : IProductValidator
    {
        public IReadOnlyList<string> ValidateForCreate(string name, string? productUrl, int? shopId)
            => ValidateCommon(name, productUrl, shopId);

        public IReadOnlyList<string> ValidateForUpdate(string name, string? productUrl, int? shopId)
            => ValidateCommon(name, productUrl, shopId);

        private static List<string> ValidateCommon(string name, string? productUrl, int? shopId)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Name is required.");
            }
            else if (name.Length > 255)
            {
                errors.Add("Name must be at most 255 characters.");
            }

            if (!string.IsNullOrWhiteSpace(productUrl) && productUrl!.Length > 2000)
            {
                errors.Add("ProductUrl must be at most 2000 characters.");
            }

            if (shopId.HasValue && shopId.Value <= 0)
            {
                errors.Add("ShopId must be a positive integer when provided.");
            }

            return errors;
        }
    }
}
