// src/Domain/PriceHistory.cs
using System;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain
{
    public class PriceHistory
    {
        private PriceHistory() {}
        public PriceHistory(
            Id id, ProductId pid,
            Id shopId, Money price,
            DateTime recordedAtUtc
        )
        {
            if (id == Id.Empty)
                throw new ArgumentException("Id ca no be empty.", nameof(id));
            Id = id;
            ProductId = pid;
            ShopId = shopId;
            Price price;
            RecordedAt = DateTime.SpecifyKind(recordedAtUtc, DateTimeKind.Utc);
        }
        public Id Id { get; private set;} // Auto-increment, PK
        public Money Price { get; private set;}
        public Datetime RecordedAt { get; private set;}
        public ProductId ProductId { get; private set;} // Product Id, FK
        public Id ShopId { get; private set;} // Shop Id, FK
        public Product? Product { get; private set;}
        public Shop? Shop { get; private set;}
    }
}