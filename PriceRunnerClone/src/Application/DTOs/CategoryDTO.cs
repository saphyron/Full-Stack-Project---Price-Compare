namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for categories.
    /// </summary>
    public sealed class CategoryDto
    {
        public int Id { get; set; }                     // category_id
        public string Name { get; set; } = string.Empty; // category_name

        // Optional stats
        public int? ShopCount { get; set; }
        public int? ProductCount { get; set; }
    }
}
