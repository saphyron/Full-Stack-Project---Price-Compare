namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for shops (butikker).
    /// Bruges til både liste og detaljer.
    /// </summary>
    public sealed class ShopDto
    {
        public int Id { get; set; }                 // shop_id
        public string FullName { get; set; } = string.Empty; // full_name
        public string? ShopUrl { get; set; }        // shop_url

        public int BrandId { get; set; }            // brand_id
        public string? BrandName { get; set; }      // brands.brand_name

        public int CategoryId { get; set; }         // category_id
        public string? CategoryName { get; set; }   // categories.category_name

        // Optional stats til data management / ML
        public int? ProductCount { get; set; }      // antal produkter i shoppen
        public int? PriceCount { get; set; }        // antal price-rows
    }
}
