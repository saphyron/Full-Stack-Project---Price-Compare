namespace PriceRunner.Api.Models
{
    // Bruges af POST /api/products
    public sealed class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }
        public int? ShopId { get; set; }
    }

    // Bruges af PUT /api/products/{id}
    public sealed class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }
        public int? ShopId { get; set; }
    }
}
