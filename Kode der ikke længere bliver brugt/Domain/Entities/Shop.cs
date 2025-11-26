// src/Domain/Shop.cs
using System;
using System.Collections.Generic;

namespace PriceRunner.Domain
{
    public class Shop
    {
        private readonly List<Price> _prices = new();
        private readonly List<PriceHistory> _priceHistory = new();
        private readonly List<Product> _products = new();

        private Shop() {}
        public Shop(string name, string? websiteUrl, int brandid, int categoryId)
        {
            SetBrand(brandid);
            SetCategory(categoryId);
            UpdateBasicInformation(name, websiteUrl);
        }
        public int Id { get; private set;} // Auto-increment, PK
        public string Name { get; private set;} = string.Empty;
        public string? WebSiteUrl { get; private set;}
        public int BrandId { get; private set;}
        public int CategoryId { get; private set;}
        public IReadOnlyCollection<Price> Prices => _prices.AsReadOnly();
        public IReadOnlyCollection<PriceHistory> PriceHistories 
            => _priceHistory.AsReadOnly();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        public void UpdateBasicInformation(string name, string? websiteUrl)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Name is required.", nameof(name))
                : name.Trim();
            WebSiteUrl = string.IsNullOrWhiteSpace(websiteUrl) ? null : websiteUrl.Trim();
        }

        public void SetBrand(int brandId)
        {
            if (brandId <= 0)
                throw new ArgumentOutOfRangeException(nameof(brandId));

            BrandId = brandId;
        }

        public void SetCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(categoryId));

            CategoryId = categoryId;
        }

        public void AddProduct(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));
            _products.Add(product);
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