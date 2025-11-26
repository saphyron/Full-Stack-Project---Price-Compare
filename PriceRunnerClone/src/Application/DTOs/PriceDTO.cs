using System;

namespace PriceRunner.Application.DTOs
{
    // Rå række fra product_prices (CRUD)
    public sealed record ProductPriceRowDto
    {
        public int Id { get; init; }              // product_price_id
        public int ProductId { get; init; }       // product_id
        public int ShopId { get; init; }          // shop_id
        public double CurrentPrice { get; init; } // current_price
        public DateTime LastUpdatedUtc { get; init; } // last_updated
    }

    // Rå række fra products_history (CRUD)
    public sealed record ProductHistoryRowDto
    {
        public int Id { get; init; }              // products_history_id
        public int ProductId { get; init; }       // product_id
        public int ShopId { get; init; }          // shop_id
        public double Price { get; init; }        // price
        public DateTime RecordedAtUtc { get; init; } // recorded_at
    }
}
