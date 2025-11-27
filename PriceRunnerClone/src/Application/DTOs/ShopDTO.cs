namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for shops (butikker).
    /// Bruges til både liste og detaljer.
    /// </summary>
    public sealed class ShopDto
    {
        public int Id { get; set; }                 // shops.shop_id
        public string FullName { get; set; } = string.Empty;
        public string? ShopUrl { get; set; }

        // Aggregated stats based on products in this shop
        public int? ProductCount { get; set; }          // DISTINCT products in the shop
        public int? BrandCount { get; set; }            // DISTINCT brands via products.brand_id
        public int? CategoryCount { get; set; }         // DISTINCT categories via products.category_id
    }

}
