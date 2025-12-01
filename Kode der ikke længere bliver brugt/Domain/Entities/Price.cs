// src/Domain/Price.cs
using System;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain
{
    public class Price
    {
        private Price()  {}
        public Price(int productId, int shopId, Money amount, DateTime lastUpdatedUtc)
        {
            if (productId <= 0) throw new ArgumentOutOfRangeException(nameof(productId));
            if (shopId <= 0) throw new ArgumentOutOfRangeException(nameof(shopId));

            ProductId = productId;
            ShopId = shopId;
            Amount = amount;
            LastUpdatedUtc = DateTime.SpecifyKind(lastUpdatedUtc, DateTimeKind.Utc);
        }
        public int Id { get; private set;} // Auto-increment PK
        public double CurrentPrice => Amount.Amount;
        public Money Amount  { get; private set;}
        public DateTime LastUpdatedUtc { get; private set;}
        public int ProductId { get; private set;} // Product Id, FK
        public int ShopId { get; private set;} // Shop Id, FK
        public Product? Product { get; private set;} // Navigation Properties
        public Shop? Shop { get; private set;} // Navigation Properties

        public void UpdateAmount(Money amount, DateTime updatedAtUtc)
        {
            Amount = amount;
            LastUpdatedUtc = DateTime.SpecifyKind(updatedAtUtc, DateTimeKind.Utc);
        }
    }
}