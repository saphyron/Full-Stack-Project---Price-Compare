using System;
using System.Collections.Generic;

namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for products.
    /// Bruges både til liste og detaljer.
    /// </summary>
    public sealed class ProductDto
    {
        // Basisfelter (liste + detail)
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        public string? BrandName { get; set; }
        public string? CategoryName { get; set; }

        // Ekstra felter (kun udfyldt ved detail-endpoints)
        public List<ProductPriceDto>? Prices { get; set; }
        public List<ProductHistoryDto>? History { get; set; }
    }

    /// <summary>
    /// DTO for current prices (product_prices).
    /// </summary>
    public sealed class ProductPriceDto
    {
        public int Id { get; set; }              // product_price_id
        public int ShopId { get; set; }          // shop_id
        public string? ShopName { get; set; }    // s.full_name
        public double Amount { get; set; }       // current_price
        public string Currency { get; set; } = "DKK";
        public DateTime? LastUpdatedUtc { get; set; } // last_updated
    }

    /// <summary>
    /// DTO for historical prices (products_history).
    /// </summary>
    public sealed class ProductHistoryDto
    {
        public int Id { get; set; }              // products_history_id
        public int ShopId { get; set; }          // shop_id
        public string? ShopName { get; set; }    // s.full_name
        public double Amount { get; set; }       // price
        public string Currency { get; set; } = "DKK";
        public DateTime? RecordedAtUtc { get; set; } // recorded_at
    }

    /// <summary>
    /// Fladt view: ét row pr. (product, shop, price).
    /// Bruges til "get all products from all shops".
    /// </summary>
    public sealed class ProductWithPriceDto
    {
        public int ProductId { get; set; }          // p.product_id
        public string ProductName { get; set; } = string.Empty; // p.product_name
        public string? ProductUrl { get; set; }     // p.product_url

        public int ShopId { get; set; }             // s.shop_id
        public string ShopName { get; set; } = string.Empty; // s.full_name

        public double Amount { get; set; }          // pp.current_price
        public string Currency { get; set; } = "DKK";
        public DateTime? LastUpdatedUtc { get; set; } // pp.last_updated
    }
}
