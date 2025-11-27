using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;
using PriceRunner.Application.Mappers;

namespace PriceRunner.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);

        Task<ProductDto> CreateAsync(string name, string? productUrl, int? shopId, int brandId, int categoryId);
        Task<bool> UpdateAsync(int id, string name, string? productUrl, int? shopId, int brandId, int categoryId);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ProductDto>> GetByShopAsync(int shopId);
        Task<IEnumerable<ProductDto>> SearchAsync(string? name);

        Task<IEnumerable<ProductWithPriceDto>> GetAllPricesAsync();
        Task<IEnumerable<ProductDto>> GetWithBrandCategoryAsync();
    }

    /// <summary>
    /// Application service for product-related operations.
    /// Encapsulates Dapper + SQL and uses PriceService for prices/history.
    /// </summary>
    public sealed class ProductService : IProductService
    {
        private readonly IDbConnection _db;
        private readonly IPriceService _priceService;

        public ProductService(IDbConnection db, IPriceService priceService)
        {
            _db = db;
            _priceService = priceService;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT 
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            ORDER BY p.product_name;";

            var items = await _db.QueryAsync<ProductDto>(sql);
            return items;
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            const string productSql = @"
            SELECT 
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            WHERE p.product_id = @Id;";

            var product = await _db.QuerySingleOrDefaultAsync<ProductDto>(productSql, new { Id = id });
            if (product is null)
                return null;

            var prices = await _priceService.GetPricesForProductAsync(id);
            var history = await _priceService.GetHistoryForProductAsync(id);

            product.WithPricesAndHistory(prices, history);
            return product;
        }

        public async Task<ProductDto> CreateAsync(string name, string? productUrl, int? shopId, int brandId, int categoryId)
        {
            const string insertSql = @"
            INSERT INTO products (product_name, product_url, shop_id, brand_id, category_id)
            VALUES (@Name, @ProductUrl, @ShopId, @BrandId, @CategoryId);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new
            {
                Name = name,
                ProductUrl = productUrl,
                ShopId = shopId,
                BrandId = brandId,
                CategoryId = categoryId
            });

            const string getSql = @"
            SELECT 
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            WHERE p.product_id = @Id;";

            var created = await _db.QuerySingleAsync<ProductDto>(getSql, new { Id = newId });
            return created;
        }

        public async Task<bool> UpdateAsync(int id, string name, string? productUrl, int? shopId, int brandId, int categoryId)
        {
            const string existsSql = "SELECT COUNT(*) FROM products WHERE product_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            const string updateSql = @"
            UPDATE products
            SET product_name = @Name,
                product_url  = @ProductUrl,
                shop_id      = @ShopId,
                brand_id     = @BrandId,
                category_id  = @CategoryId
            WHERE product_id = @Id;";

            await _db.ExecuteAsync(updateSql, new
            {
                Id = id,
                Name = name,
                ProductUrl = productUrl,
                ShopId = shopId,
                BrandId = brandId,
                CategoryId = categoryId
            });

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM products WHERE product_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<ProductDto>> GetByShopAsync(int shopId)
        {
            const string sql = @"
            SELECT 
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            WHERE p.shop_id = @ShopId
            ORDER BY p.product_name;";

            var items = await _db.QueryAsync<ProductDto>(sql, new { ShopId = shopId });
            return items;
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string? name)
        {
            const string sql = @"
            SELECT 
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            WHERE (@Name IS NULL OR p.product_name LIKE CONCAT('%', @Name, '%'))
            ORDER BY p.product_name;";

            var items = await _db.QueryAsync<ProductDto>(sql, new { Name = name });
            return items;
        }

        public async Task<IEnumerable<ProductWithPriceDto>> GetAllPricesAsync()
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
            ORDER BY p.product_name, s.full_name;";

            var rows = await _db.QueryAsync<ProductWithPriceDto>(sql);
            return rows;
        }

        public async Task<IEnumerable<ProductDto>> GetWithBrandCategoryAsync()
        {
            const string sql = @"
            SELECT
                p.product_id      AS Id,
                p.product_name    AS Name,
                p.product_url     AS ProductUrl,
                p.shop_id         AS ShopId,
                s.full_name       AS ShopName,
                p.brand_id        AS BrandId,
                b.brand_name      AS BrandName,
                p.category_id     AS CategoryId,
                c.category_name   AS CategoryName
            FROM products p
            LEFT JOIN shops      s ON s.shop_id      = p.shop_id
            INNER JOIN brands    b ON b.brand_id     = p.brand_id
            INNER JOIN categories c ON c.category_id = p.category_id
            ORDER BY p.product_name;";

            var items = await _db.QueryAsync<ProductDto>(sql);
            return items;
        }
    }
}
