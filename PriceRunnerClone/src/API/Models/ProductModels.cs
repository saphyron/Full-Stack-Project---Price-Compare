namespace PriceRunner.Api.Models
{
    // Bruges af POST /api/products
    public sealed class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }

        // Nullable, fordi products.shop_id i DB er optional
        public int? ShopId { get; set; }

        // IKKE nullable – products.brand_id / category_id er NOT NULL i schema
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }

    // Bruges af PUT /api/products/{id}
    public sealed class UpdateProductRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }

        public int? ShopId { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
