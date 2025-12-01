namespace PriceRunner.Api.Models
{
    public sealed class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public sealed class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
