using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IPriceService
    {
        // Allerede brugt af ProductEndpoint
        Task<IEnumerable<ProductPriceDto>> GetPricesForProductAsync(int productId);
        Task<ProductPriceDto?> GetCheapestPriceForProductAsync(int productId);
        Task<IEnumerable<ProductHistoryDto>> GetHistoryForProductAsync(int productId);

        // NYT: CRUD over product_prices
        Task<IEnumerable<ProductPriceRowDto>> GetAllPriceRowsAsync();
        Task<ProductPriceRowDto?> GetPriceRowByIdAsync(int id);
        Task<ProductPriceRowDto> CreatePriceRowAsync(
            int productId,
            int shopId,
            double currentPrice,
            DateTime? lastUpdatedUtc);
        Task<bool> UpdatePriceRowAsync(
            int id,
            int productId,
            int shopId,
            double currentPrice,
            DateTime? lastUpdatedUtc);
        Task<bool> DeletePriceRowAsync(int id);

        // NYT: CRUD over products_history
        Task<IEnumerable<ProductHistoryRowDto>> GetAllHistoryRowsAsync();
        Task<ProductHistoryRowDto?> GetHistoryRowByIdAsync(int id);
        Task<ProductHistoryRowDto> CreateHistoryRowAsync(
            int productId,
            int shopId,
            double price,
            DateTime? recordedAtUtc);
        Task<bool> UpdateHistoryRowAsync(
            int id,
            int productId,
            int shopId,
            double price,
            DateTime? recordedAtUtc);
        Task<bool> DeleteHistoryRowAsync(int id);
    }

    /// <summary>
    /// Service responsible for price- and history-related queries.
    /// Encapsulates Dapper + SQL for product_prices / products_history.
    /// </summary>
    public sealed class PriceService : IPriceService
    {
        private readonly IDbConnection _db;

        public PriceService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ProductPriceDto>> GetPricesForProductAsync(int productId)
        {
            const string sql = @"
            SELECT
                pp.product_price_id AS Id,
                pp.shop_id          AS ShopId,
                s.full_name         AS ShopName,
                pp.current_price    AS Amount,
                'DKK'               AS Currency,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            INNER JOIN shops s ON s.shop_id = pp.shop_id
            WHERE pp.product_id = @Id
            ORDER BY pp.current_price ASC;";

            var prices = await _db.QueryAsync<ProductPriceDto>(sql, new { Id = productId });
            return prices;
        }

        public async Task<ProductPriceDto?> GetCheapestPriceForProductAsync(int productId)
        {
            const string sql = @"
            SELECT
                pp.product_price_id AS Id,
                pp.shop_id          AS ShopId,
                s.full_name         AS ShopName,
                pp.current_price    AS Amount,
                'DKK'               AS Currency,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            INNER JOIN shops s ON s.shop_id = pp.shop_id
            WHERE pp.product_id = @Id
            ORDER BY pp.current_price ASC
            LIMIT 1;";

            var cheapest = await _db.QuerySingleOrDefaultAsync<ProductPriceDto>(sql, new { Id = productId });
            return cheapest;
        }

        public async Task<IEnumerable<ProductHistoryDto>> GetHistoryForProductAsync(int productId)
        {
            const string sql = @"
            SELECT
                ph.products_history_id AS Id,
                ph.shop_id             AS ShopId,
                s.full_name            AS ShopName,
                ph.price               AS Amount,
                'DKK'                  AS Currency,
                ph.recorded_at         AS RecordedAtUtc
            FROM products_history ph
            INNER JOIN shops s ON s.shop_id = ph.shop_id
            WHERE ph.product_id = @Id
            ORDER BY ph.recorded_at DESC;";

            var history = await _db.QueryAsync<ProductHistoryDto>(sql, new { Id = productId });
            return history;
        }

        public async Task<IEnumerable<ProductPriceRowDto>> GetAllPriceRowsAsync()
        {
            const string sql = @"
            SELECT
                pp.product_price_id AS Id,
                pp.product_id       AS ProductId,
                pp.shop_id          AS ShopId,
                pp.current_price    AS CurrentPrice,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            ORDER BY pp.product_id, pp.shop_id;";

            return await _db.QueryAsync<ProductPriceRowDto>(sql);
        }

        public async Task<ProductPriceRowDto?> GetPriceRowByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                pp.product_price_id AS Id,
                pp.product_id       AS ProductId,
                pp.shop_id          AS ShopId,
                pp.current_price    AS CurrentPrice,
                pp.last_updated     AS LastUpdatedUtc
            FROM product_prices pp
            WHERE pp.product_price_id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<ProductPriceRowDto>(sql, new { Id = id });
        }

        public async Task<ProductPriceRowDto> CreatePriceRowAsync(
            int productId,
            int shopId,
            double currentPrice,
            DateTime? lastUpdatedUtc)
        {
            var last = lastUpdatedUtc ?? DateTime.UtcNow;

            const string insertSql = @"
            INSERT INTO product_prices (current_price, last_updated, product_id, shop_id)
            VALUES (@CurrentPrice, @LastUpdatedUtc, @ProductId, @ShopId);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new
            {
                CurrentPrice = currentPrice,
                LastUpdatedUtc = last,
                ProductId = productId,
                ShopId = shopId
            });

            var created = await GetPriceRowByIdAsync(newId);
            if (created is null)
                throw new InvalidOperationException("Inserted product price row not found.");

            return created;
        }

        public async Task<bool> UpdatePriceRowAsync(
            int id,
            int productId,
            int shopId,
            double currentPrice,
            DateTime? lastUpdatedUtc)
        {
            var last = lastUpdatedUtc ?? DateTime.UtcNow;

            const string sql = @"
            UPDATE product_prices
            SET current_price = @CurrentPrice,
                last_updated  = @LastUpdatedUtc,
                product_id    = @ProductId,
                shop_id       = @ShopId
            WHERE product_price_id = @Id;";

            var rows = await _db.ExecuteAsync(sql, new
            {
                Id = id,
                CurrentPrice = currentPrice,
                LastUpdatedUtc = last,
                ProductId = productId,
                ShopId = shopId
            });

            return rows > 0;
        }

        public async Task<bool> DeletePriceRowAsync(int id)
        {
            const string sql = @"DELETE FROM product_prices WHERE product_price_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<ProductHistoryRowDto>> GetAllHistoryRowsAsync()
        {
            const string sql = @"
            SELECT
                ph.products_history_id AS Id,
                ph.product_id          AS ProductId,
                ph.shop_id             AS ShopId,
                ph.price               AS Price,
                ph.recorded_at         AS RecordedAtUtc
            FROM products_history ph
            ORDER BY ph.recorded_at;";

            return await _db.QueryAsync<ProductHistoryRowDto>(sql);
        }

        public async Task<ProductHistoryRowDto?> GetHistoryRowByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                ph.products_history_id AS Id,
                ph.product_id          AS ProductId,
                ph.shop_id             AS ShopId,
                ph.price               AS Price,
                ph.recorded_at         AS RecordedAtUtc
            FROM products_history ph
            WHERE ph.products_history_id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<ProductHistoryRowDto>(sql, new { Id = id });
        }

        public async Task<ProductHistoryRowDto> CreateHistoryRowAsync(
            int productId,
            int shopId,
            double price,
            DateTime? recordedAtUtc)
        {
            var recorded = recordedAtUtc ?? DateTime.UtcNow;

            const string insertSql = @"
            INSERT INTO products_history (price, recorded_at, product_id, shop_id)
            VALUES (@Price, @RecordedAtUtc, @ProductId, @ShopId);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new
            {
                Price = price,
                RecordedAtUtc = recorded,
                ProductId = productId,
                ShopId = shopId
            });

            var created = await GetHistoryRowByIdAsync(newId);
            if (created is null)
                throw new InvalidOperationException("Inserted product history row not found.");

            return created;
        }

        public async Task<bool> UpdateHistoryRowAsync(
            int id,
            int productId,
            int shopId,
            double price,
            DateTime? recordedAtUtc)
        {
            var recorded = recordedAtUtc ?? DateTime.UtcNow;

            const string sql = @"
            UPDATE products_history
            SET price       = @Price,
                recorded_at = @RecordedAtUtc,
                product_id  = @ProductId,
                shop_id     = @ShopId
            WHERE products_history_id = @Id;";

            var rows = await _db.ExecuteAsync(sql, new
            {
                Id = id,
                Price = price,
                RecordedAtUtc = recorded,
                ProductId = productId,
                ShopId = shopId
            });

            return rows > 0;
        }

        public async Task<bool> DeleteHistoryRowAsync(int id)
        {
            const string sql = @"DELETE FROM products_history WHERE products_history_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
