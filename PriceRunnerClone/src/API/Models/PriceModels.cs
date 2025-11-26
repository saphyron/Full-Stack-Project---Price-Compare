using System;

namespace PriceRunner.Api.Models
{
    // product_prices
    public sealed class CreateProductPriceRequest
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }
    }

    public sealed class UpdateProductPriceRequest
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }
    }

    // products_history
    public sealed class CreateProductHistoryRequest
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public double Price { get; set; }
        public DateTime? RecordedAtUtc { get; set; }
    }

    public sealed class UpdateProductHistoryRequest
    {
        public int ProductId { get; set; }
        public int ShopId { get; set; }
        public double Price { get; set; }
        public DateTime? RecordedAtUtc { get; set; }
    }
}
