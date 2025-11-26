// src/Domain/PriceHistory.cs
using System;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain
{
    public class PriceHistory
    {
        private PriceHistory() {}
        public PriceHistory(
            int productId,
            int shopId,
            Money price,
            DateTime recordedAtUtc)
        {
            if (productId <= 0) throw new ArgumentOutOfRangeException(nameof(productId));
            if (shopId <= 0) throw new ArgumentOutOfRangeException(nameof(shopId));

            ProductId = productId;
            ShopId = shopId;
            Price = price;
            RecordedAt = DateTime.SpecifyKind(recordedAtUtc, DateTimeKind.Utc);
        }
        public int Id { get; private set;} // Auto-increment, PK
        public Money Price { get; private set;}
        public DateTime RecordedAt { get; private set;}
        public int ProductId { get; private set;} // Product Id, FK
        public int ShopId { get; private set;} // Shop Id, FK
        public Product? Product { get; private set;}
        public Shop? Shop { get; private set;}
    }
}