namespace PriceRunner.Api.Models
{
    public sealed class CreateShopRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? ShopUrl { get; set; }
    }

    public sealed class UpdateShopRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? ShopUrl { get; set; }
    }
}
