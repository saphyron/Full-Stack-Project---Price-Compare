// src/Domain/Price.cs
using System;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain
{
    public class Price
    {
        private Price()  {}
        public Price(Id id, ProductId pid, Id shopId, 
                        Money amount, DateTime lastUpdateUTC)
        {
            if (id == Id.Empty)
                throw new ArgumentException("Id can not be empty.", nameof(id));
            Guid = id;
            ProductId = pid;
            ShopId = shopId;
            Amount = amount;
            LastUpdated = DateTime.SpecifyKind(lastUpdateUTC, DateTimeKind.Utc);
        }
        public Id Guid { get; private set;} // Auto-increment PK
        public Money Amount  { get; private set;}
        public DateTime LastUpdated { get; private set;}
        public ProductId ProductId { get; private set;} // Product Id, FK
        public Id ShopId { get; private set;} // Shop Id, FK
        public Product? Product { get; private set;} // Navigation Properties
        public Shop? Shop { get; private set;} // Navigation Properties

        public void UpdateAmount(Money amount, DateTime updatedAtUtc)
        {
            Amount = amount;
            LastUpdated = DateTime.SpecifyKind(updatedAtUtc, DateTimeKind.Utc);
        }
    }
}