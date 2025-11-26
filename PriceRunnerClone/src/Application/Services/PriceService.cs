using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IPriceService
    {
        Task<IEnumerable<ProductPriceDto>> GetPricesForProductAsync(int productId);
        Task<ProductPriceDto?> GetCheapestPriceForProductAsync(int productId);
        Task<IEnumerable<ProductHistoryDto>> GetHistoryForProductAsync(int productId);
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
    }
}
