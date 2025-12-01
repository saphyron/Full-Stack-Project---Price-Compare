namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for brands.
    /// </summary>
    public sealed class BrandDto
    {
        public int Id { get; set; }                 // brand_id
        public string Name { get; set; } = string.Empty; // brand_name

        // Optional stats
        public int? ShopCount { get; set; }         // antal shops tilknyttet
        public int? ProductCount { get; set; }      // antal produkter via shops
    }
}
