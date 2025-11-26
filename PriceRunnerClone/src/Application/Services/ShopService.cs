using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IShopService
    {
        Task<IEnumerable<ShopDto>> GetAllAsync();
        Task<ShopDto?> GetByIdAsync(int id);
        Task<ShopDto> CreateAsync(string fullName, string? shopUrl, int brandId, int categoryId);
        Task<bool> UpdateAsync(int id, string fullName, string? shopUrl, int brandId, int categoryId);
        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ProductDto>> GetProductsAsync(int shopId);
        Task<IEnumerable<ProductWithPriceDto>> GetPricesAsync(int shopId);
    }

    public sealed class ShopService : IShopService
    {
        private readonly IDbConnection _db;

        public ShopService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ShopDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                s.shop_id       AS Id,
                s.full_name     AS FullName,
                s.shop_url      AS ShopUrl,
                s.brand_id      AS BrandId,
                b.brand_name    AS BrandName,
                s.category_id   AS CategoryId,
                c.category_name AS CategoryName
            FROM shops s
            LEFT JOIN brands b     ON b.brand_id     = s.brand_id
            LEFT JOIN categories c ON c.category_id  = s.category_id
            ORDER BY s.full_name;";

            return await _db.QueryAsync<ShopDto>(sql);
        }

        public async Task<ShopDto?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                s.shop_id       AS Id,
                s.full_name     AS FullName,
                s.shop_url      AS ShopUrl,
                s.brand_id      AS BrandId,
                b.brand_name    AS BrandName,
                s.category_id   AS CategoryId,
                c.category_name AS CategoryName
            FROM shops s
            LEFT JOIN brands b     ON b.brand_id     = s.brand_id
            LEFT JOIN categories c ON c.category_id  = s.category_id
            WHERE s.shop_id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<ShopDto>(sql, new { Id = id });
        }

        public async Task<ShopDto> CreateAsync(string fullName, string? shopUrl, int brandId, int categoryId)
        {
            const string insertSql = @"
            INSERT INTO shops (full_name, shop_url, brand_id, category_id)
            VALUES (@FullName, @ShopUrl, @BrandId, @CategoryId);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new
            {
                FullName = fullName,
                ShopUrl = shopUrl,
                BrandId = brandId,
                CategoryId = categoryId
            });

            var created = await GetByIdAsync(newId);
            // created burde aldrig være null her
            return created!;
        }

        public async Task<bool> UpdateAsync(int id, string fullName, string? shopUrl, int brandId, int categoryId)
        {
            const string existsSql = "SELECT COUNT(*) FROM shops WHERE shop_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            const string updateSql = @"
            UPDATE shops
            SET full_name   = @FullName,
                shop_url    = @ShopUrl,
                brand_id    = @BrandId,
                category_id = @CategoryId
            WHERE shop_id = @Id;";

            await _db.ExecuteAsync(updateSql, new
            {
                Id = id,
                FullName = fullName,
                ShopUrl = shopUrl,
                BrandId = brandId,
                CategoryId = categoryId
            });

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM shops WHERE shop_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int shopId)
        {
            const string sql = @"
            SELECT
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                b.brand_name      AS BrandName,
                c.category_name   AS CategoryName
            FROM products p
            INNER JOIN shops s      ON s.shop_id     = p.shop_id
            LEFT JOIN brands b      ON b.brand_id    = s.brand_id
            LEFT JOIN categories c  ON c.category_id = s.category_id
            WHERE p.shop_id = @ShopId
            ORDER BY p.product_name;";

            return await _db.QueryAsync<ProductDto>(sql, new { ShopId = shopId });
        }

        public async Task<IEnumerable<ProductWithPriceDto>> GetPricesAsync(int shopId)
        {
            const string sql = @"
            SELECT
                p.product_id        AS ProductId,
                p.product_name      AS ProductName,
                p.product_url       AS ProductUrl,
                s.shop_id           AS ShopId,
                s.full_name         AS ShopName,
                pp.current_price    AS Amount,
                'DKK'               AS Currency,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            INNER JOIN products p ON p.product_id = pp.product_id
            INNER JOIN shops    s ON s.shop_id    = pp.shop_id
            WHERE s.shop_id = @ShopId
            ORDER BY p.product_name;";

            return await _db.QueryAsync<ProductWithPriceDto>(sql, new { ShopId = shopId });
        }
    }
}
