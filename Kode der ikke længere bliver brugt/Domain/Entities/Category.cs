// src/Domain/Category.cs
using System;
using System.Collections.Generic;

namespace PriceRunner.Domain
{
    public class Category
    {
        private readonly List<Shop> _shops = new();

        // For ORM
        private Category() { }

        public Category(string name)
        {
            UpdateName(name);
        }

        public int Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public IReadOnlyCollection<Shop> Shops => _shops.AsReadOnly();

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.", nameof(name));

            Name = name.Trim();
        }

        internal void AddShop(Shop shop)
        {
            if (shop is null) throw new ArgumentNullException(nameof(shop));
            _shops.Add(shop);
        }
    }
}
