// src/Domain/Shop.cs
using System;
using System.Collections.Generic;

namespace PriceRunner.Domain
{
    public class Shop
    {
        private readonly List<Price> _prices = new();
        private readonly List<PriceHistory> _priceHistory = new();

        private Shop() {}
        public Shop(Id id, string name, string? websiteUrl)
        {
            Id = id == SId.Empty
                ? throw new ArgumentException("Id cannot be Empty", nameof(id)) 
                : id;
            UpdateBasicInformation(name, websiteUrl);
        }
        public SId Id { get; private set;} // Auto-increment, PK
        public string Name { get; private set;} = string.Empty;
        public string? WebSiteUrl { get; private set;}
        public IReadOnlyCollection<Price> Prices => _prices.AsReadOnly();
        public IReadOnlyCollection<PriceHistory> PriceHistories 
            => _priceHistory.AsReadOnly();

        public void UpdateBasicInformation(string name, string? websiteUrl)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Name is required.", nameof(name))
                : name.Trim();
            WebSiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl.Trim();
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