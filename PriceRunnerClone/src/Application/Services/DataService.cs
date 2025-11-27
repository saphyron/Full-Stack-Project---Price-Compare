using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IDataService
    {
        Task<IEnumerable<ProductFlatDataDto>> GetProductsFlatAsync();
        Task<IEnumerable<PriceHistoryDataDto>> GetPriceHistoryAsync(
            int? productId,
            int? shopId,
            DateTime? from,
            DateTime? to);
        Task<IEnumerable<ShopStatsDto>> GetShopStatsAsync();
        Task<IEnumerable<BrandStatsDto>> GetBrandStatsAsync();
        Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync();
    }

    public sealed class DataService : IDataService
    {
        private readonly IDbConnection _db;

        public DataService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductFlatDataDto>> GetProductsFlatAsync()
        {
            const string sql = @"
            SELECT
                p.product_id        AS ProductId,
                p.product_name      AS ProductName,
                p.product_url       AS ProductUrl,

                s.shop_id           AS ShopId,
                s.full_name         AS ShopName,

                b.brand_id          AS BrandId,
                b.brand_name        AS BrandName,

                c.category_id       AS CategoryId,
                c.category_name     AS CategoryName,

                pp.current_price    AS Amount,
                'DKK'               AS Currency,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            INNER JOIN products   p ON p.product_id   = pp.product_id
            INNER JOIN shops      s ON s.shop_id      = pp.shop_id
            INNER JOIN brands     b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id  = p.category_id
            ORDER BY p.product_name, s.full_name;";

            return await _db.QueryAsync<ProductFlatDataDto>(sql);
        }

        public async Task<IEnumerable<PriceHistoryDataDto>> GetPriceHistoryAsync(
            int? productId,
            int? shopId,
            DateTime? from,
            DateTime? to)
        {
            const string sql = @"
            SELECT
                ph.products_history_id AS ProductHistoryId,

                p.product_id        AS ProductId,
                p.product_name      AS ProductName,

                s.shop_id           AS ShopId,
                s.full_name         AS ShopName,

                b.brand_id          AS BrandId,
                b.brand_name        AS BrandName,

                c.category_id       AS CategoryId,
                c.category_name     AS CategoryName,

                ph.price            AS Amount,
                'DKK'               AS Currency,
                ph.recorded_at      AS RecordedAtUtc
            FROM products_history ph
            INNER JOIN products   p ON p.product_id   = ph.product_id
            INNER JOIN shops      s ON s.shop_id      = ph.shop_id
            INNER JOIN brands     b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id  = p.category_id
            WHERE (@ProductId IS NULL OR ph.product_id = @ProductId)
              AND (@ShopId    IS NULL OR ph.shop_id    = @ShopId)
              AND (@FromDate  IS NULL OR ph.recorded_at >= @FromDate)
              AND (@ToDate    IS NULL OR ph.recorded_at <= @ToDate)
            ORDER BY ph.recorded_at;";

            return await _db.QueryAsync<PriceHistoryDataDto>(sql, new
            {
                ProductId = productId,
                ShopId = shopId,
                FromDate = from,
                ToDate = to
            });
        }

        public async Task<IEnumerable<ShopStatsDto>> GetShopStatsAsync()
        {
            const string sql = @"
            SELECT
                s.shop_id           AS ShopId,
                s.full_name         AS ShopName,

                COUNT(DISTINCT p.product_id)        AS ProductCount,
                COUNT(DISTINCT pp.product_price_id) AS PriceRowCount,

                MIN(pp.current_price) AS MinPrice,
                MAX(pp.current_price) AS MaxPrice,
                AVG(pp.current_price) AS AvgPrice
            FROM shops s
            LEFT JOIN products       p  ON p.shop_id  = s.shop_id
            LEFT JOIN product_prices pp ON pp.shop_id = s.shop_id
            GROUP BY
                s.shop_id,
                s.full_name
            ORDER BY s.full_name;";

            return await _db.QueryAsync<ShopStatsDto>(sql);
        }

        public async Task<IEnumerable<BrandStatsDto>> GetBrandStatsAsync()
        {
            const string sql = @"
            SELECT
                b.brand_id          AS BrandId,
                b.brand_name        AS BrandName,

                COUNT(DISTINCT s.shop_id)           AS ShopCount,
                COUNT(DISTINCT p.product_id)        AS ProductCount,
                COUNT(DISTINCT pp.product_price_id) AS PriceRowCount,

                MIN(pp.current_price) AS MinPrice,
                MAX(pp.current_price) AS MaxPrice,
                AVG(pp.current_price) AS AvgPrice
            FROM brands b
            LEFT JOIN products       p  ON p.brand_id = b.brand_id
            LEFT JOIN shops          s  ON s.shop_id  = p.shop_id
            LEFT JOIN product_prices pp ON pp.product_id = p.product_id
            GROUP BY
                b.brand_id,
                b.brand_name
            ORDER BY b.brand_name;";

            return await _db.QueryAsync<BrandStatsDto>(sql);
        }

        public async Task<IEnumerable<CategoryStatsDto>> GetCategoryStatsAsync()
        {
            const string sql = @"
            SELECT
                c.category_id       AS CategoryId,
                c.category_name     AS CategoryName,

                COUNT(DISTINCT s.shop_id)           AS ShopCount,
                COUNT(DISTINCT p.product_id)        AS ProductCount,
                COUNT(DISTINCT pp.product_price_id) AS PriceRowCount,

                MIN(pp.current_price) AS MinPrice,
                MAX(pp.current_price) AS MaxPrice,
                AVG(pp.current_price) AS AvgPrice
            FROM categories c
            LEFT JOIN products       p  ON p.category_id = c.category_id
            LEFT JOIN shops          s  ON s.shop_id     = p.shop_id
            LEFT JOIN product_prices pp ON pp.product_id = p.product_id
            GROUP BY
                c.category_id,
                c.category_name
            ORDER BY c.category_name;";

            return await _db.QueryAsync<CategoryStatsDto>(sql);
        }
    }
}
