// src/Domain/Product.cs
using System;
using System.Collections.Generic;

namespace PriceRunner.Domain
{
    public class Product
    {
        private readonly List<Price> _prices = new();
        private readonly List<PriceHistory> _priceHistory = new();
        private Product() {}
        public Product(string name, string? imageUrl = null, int? shopId = null)
        {
            UpdateBasicInformation(name, imageUrl);
            SetShop(shopId);
        }
        public ProductId Id { get; private set;} // Auto-increment, PK
        public string Name { get; private set;} = string.Empty;
        public string? ImageUrl { get; private set;} = string.Empty;

        public IReadOnlyCollection<Price> Prices => _prices.AsReadOnly();
        public IReadOnlyCollection<PriceHistory> PriceHistory => 
            _priceHistory.AsReadOnly();
        
        public void UpdateBasicInfo(string name, string? productUrl)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Name is required.", nameof(name))
                : name.Trim();

            ProductUrl = string.IsNullOrWhiteSpace(productUrl) ? null : productUrl.Trim();
        }

        public void SetShop(int? shopId)
        {
            if (shopId.HasValue && shopId.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(shopId));

            ShopId = shopId;
        }

        public void AddPrice(Price price)
        {
            if (price is null) throw new ArgumentNullException(nameof(price));
            _prices.Add(price);
        }

        public void AddPriceHistory(PriceHistory entry)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));
            _priceHistory.Add(entry);
        }
    }
}