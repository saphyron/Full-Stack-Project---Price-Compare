using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> CreateAsync(string name);
        Task<bool> UpdateAsync(int id, string name);
        Task<bool> DeleteAsync(int id);
    }

    public sealed class CategoryService : ICategoryService
    {
        private readonly IDbConnection _db;

        public CategoryService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                c.category_id      AS Id,
                c.category_name    AS Name,
                COUNT(DISTINCT s.shop_id)    AS ShopCount,
                COUNT(DISTINCT p.product_id) AS ProductCount
            FROM categories c
            LEFT JOIN products p ON p.category_id = c.category_id
            LEFT JOIN shops    s ON s.shop_id     = p.shop_id
            GROUP BY c.category_id, c.category_name
            ORDER BY c.category_name;";

            return await _db.QueryAsync<CategoryDto>(sql);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                c.category_id      AS Id,
                c.category_name    AS Name,
                COUNT(DISTINCT s.shop_id)    AS ShopCount,
                COUNT(DISTINCT p.product_id) AS ProductCount
            FROM categories c
            LEFT JOIN products p ON p.category_id = c.category_id
            LEFT JOIN shops    s ON s.shop_id     = p.shop_id
            WHERE c.category_id = @Id
            GROUP BY c.category_id, c.category_name;";

            return await _db.QuerySingleOrDefaultAsync<CategoryDto>(sql, new { Id = id });
        }

        public async Task<CategoryDto> CreateAsync(string name)
        {
            const string insertSql = @"
            INSERT INTO categories (category_name)
            VALUES (@Name);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new { Name = name });

            const string getSql = @"
            SELECT
                c.category_id   AS Id,
                c.category_name AS Name,
                0               AS ShopCount,
                0               AS ProductCount
            FROM categories c
            WHERE c.category_id = @Id;";

            return await _db.QuerySingleAsync<CategoryDto>(getSql, new { Id = newId });
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            const string existsSql = "SELECT COUNT(*) FROM categories WHERE category_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            const string sql = @"
            UPDATE categories
            SET category_name = @Name
            WHERE category_id = @Id;";

            await _db.ExecuteAsync(sql, new { Id = id, Name = name });
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM categories WHERE category_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
