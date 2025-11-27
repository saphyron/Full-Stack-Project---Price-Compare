using System;

namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// Fladt datasæt til ML:
    /// ét row pr. (product, shop, current price) + brand + category.
    /// </summary>
    public sealed class ProductFlatDataDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductUrl { get; set; }

        public int ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public double Amount { get; set; }
        public string Currency { get; set; } = "DKK";
        public DateTime? LastUpdatedUtc { get; set; }
    }

    /// <summary>
    /// Time-series datasæt: price history med product, shop, brand, category.
    /// </summary>
    public sealed class PriceHistoryDataDto
    {
        public int ProductHistoryId { get; set; } // products_history_id

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public double Amount { get; set; }
        public string Currency { get; set; } = "DKK";
        public DateTime? RecordedAtUtc { get; set; }
    }

    /// <summary>
    /// Aggregerede stats per shop.
    /// </summary>
    public sealed class ShopStatsDto
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;

        public int ProductCount { get; set; }
        public int PriceRowCount { get; set; }

        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? AvgPrice { get; set; }
    }


    /// <summary>
    /// Aggregerede stats per brand.
    /// </summary>
    public sealed class BrandStatsDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        public int ShopCount { get; set; }
        public int ProductCount { get; set; }
        public int PriceRowCount { get; set; }

        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? AvgPrice { get; set; }
    }

    /// <summary>
    /// Aggregerede stats per category.
    /// </summary>
    public sealed class CategoryStatsDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public int ShopCount { get; set; }
        public int ProductCount { get; set; }
        public int PriceRowCount { get; set; }

        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? AvgPrice { get; set; }
    }
}
