// src/Domain/Product.cs
using System;
using System.Collections.Generic;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain
{
    public class Product
    {
        private readonly List<Price> _prices = new();
        private Product() {}
        public Product(ProductId id, string name, string? brand = null, 
                        string category, string? imageUrl = null)
        {
            Id = id;
            UpdateBasicInformation(name, brand, category, imageUrl);
        }
        public ProductId Id { get; private set;} // Auto-increment, PK
        public string Name { get; private set;} = string.Empty;
        public string? Brand { get; private set;}
        public string Category { get; private set;} = string.Empty;
        public string? ImageUrl { get; private set;} = string.Empty;

        public IReadOnlyCollection<Price> Prices => _prices.AsReadOnly();
        public IReadOnlyCollection<PriceHistory> PriceHistory => 
            _priceHistory.AsReadOnly();
        
        public void UpdateBasicInformation(string name, string? brand, 
                                            string category, string? imageUrl)
        {
            Name = string.IsNullOrWhiteSpace(name) 
                ? throw new ArgumentException("Name is required.", nameof(name))
                : name.Trim();
            Brand = string.IsNullOrWhiteSpace(brand) ? null : brand.Trim();
            Category = string.IsNullOrWhiteSpace(category) 
                ? throw new ArgumentException("category is required.", nameof(category))
                : category.Trim();
            ImageUrl = string.IsNullOrWhiteSpace(imageUrl) ? null : imageUrl.Trim();
        }

        public void AddPrice(Price price)
        {
            if (price is null) throw new ArgumentNullException(nameof(price));
            _prices.Add(price);
        }

        public void AddPriceHistory(PriceHistory entry)
        {
            if (entry is null) throw new ArgumentNullException(nameof(entry));
            _priceHistory.add(entry);
        }
    }
}