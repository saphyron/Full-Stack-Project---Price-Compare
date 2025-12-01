using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto?> GetByIdAsync(int id);
        Task<BrandDto> CreateAsync(string name);
        Task<bool> UpdateAsync(int id, string name);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ShopDto>> GetShopsAsync(int brandId);
    }

    public sealed class BrandService : IBrandService
    {
        private readonly IDbConnection _db;

        public BrandService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                b.brand_id     AS Id,
                b.brand_name   AS Name,
                COUNT(DISTINCT s.shop_id)    AS ShopCount,
                COUNT(DISTINCT p.product_id) AS ProductCount
            FROM brands b
            LEFT JOIN shops    s ON s.brand_id = b.brand_id
            LEFT JOIN products p ON p.shop_id  = s.shop_id
            GROUP BY b.brand_id, b.brand_name
            ORDER BY b.brand_name;";

            return await _db.QueryAsync<BrandDto>(sql);
        }

        public async Task<BrandDto?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                b.brand_id     AS Id,
                b.brand_name   AS Name,
                COUNT(DISTINCT s.shop_id)    AS ShopCount,
                COUNT(DISTINCT p.product_id) AS ProductCount
            FROM brands b
            LEFT JOIN shops    s ON s.brand_id = b.brand_id
            LEFT JOIN products p ON p.shop_id  = s.shop_id
            WHERE b.brand_id = @Id
            GROUP BY b.brand_id, b.brand_name;";

            return await _db.QuerySingleOrDefaultAsync<BrandDto>(sql, new { Id = id });
        }

        public async Task<BrandDto> CreateAsync(string name)
        {
            const string insertSql = @"
            INSERT INTO brands (brand_name)
            VALUES (@Name);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new { Name = name });

            const string getSql = @"
            SELECT
                b.brand_id   AS Id,
                b.brand_name AS Name,
                0            AS ShopCount,
                0            AS ProductCount
            FROM brands b
            WHERE b.brand_id = @Id;";

            return await _db.QuerySingleAsync<BrandDto>(getSql, new { Id = newId });
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            const string existsSql = "SELECT COUNT(*) FROM brands WHERE brand_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            const string sql = @"
            UPDATE brands
            SET brand_name = @Name
            WHERE brand_id = @Id;";

            await _db.ExecuteAsync(sql, new { Id = id, Name = name });
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM brands WHERE brand_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<ShopDto>> GetShopsAsync(int brandId)
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
            WHERE s.brand_id = @BrandId
            ORDER BY s.full_name;";

            return await _db.QueryAsync<ShopDto>(sql, new { BrandId = brandId });
        }
    }
}
