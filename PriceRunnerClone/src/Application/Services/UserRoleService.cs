using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleDto>> GetAllAsync();
        Task<UserRoleDto?> GetByIdAsync(int id);
        Task<UserRoleDto> CreateAsync(string name);
        Task<bool> UpdateAsync(int id, string name);
        Task<bool> DeleteAsync(int id);
    }

    public sealed class UserRoleService : IUserRoleService
    {
        private readonly IDbConnection _db;

        public UserRoleService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserRoleDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                ur.user_role_id   AS Id,
                ur.user_role_name AS Name,
                COUNT(u.user_id)  AS UserCount
            FROM user_roles ur
            LEFT JOIN users u ON u.user_role_id = ur.user_role_id
            GROUP BY ur.user_role_id, ur.user_role_name
            ORDER BY ur.user_role_name;";

            return await _db.QueryAsync<UserRoleDto>(sql);
        }

        public async Task<UserRoleDto?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                ur.user_role_id   AS Id,
                ur.user_role_name AS Name,
                COUNT(u.user_id)  AS UserCount
            FROM user_roles ur
            LEFT JOIN users u ON u.user_role_id = ur.user_role_id
            WHERE ur.user_role_id = @Id
            GROUP BY ur.user_role_id, ur.user_role_name;";

            return await _db.QuerySingleOrDefaultAsync<UserRoleDto>(sql, new { Id = id });
        }

        public async Task<UserRoleDto> CreateAsync(string name)
        {
            const string insertSql = @"
            INSERT INTO user_roles (user_role_name)
            VALUES (@Name);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new { Name = name });

            const string getSql = @"
            SELECT
                ur.user_role_id   AS Id,
                ur.user_role_name AS Name,
                0                 AS UserCount
            FROM user_roles ur
            WHERE ur.user_role_id = @Id;";

            return await _db.QuerySingleAsync<UserRoleDto>(getSql, new { Id = newId });
        }

        public async Task<bool> UpdateAsync(int id, string name)
        {
            const string existsSql = "SELECT COUNT(*) FROM user_roles WHERE user_role_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            const string sql = @"
            UPDATE user_roles
            SET user_role_name = @Name
            WHERE user_role_id = @Id;";

            await _db.ExecuteAsync(sql, new { Id = id, Name = name });
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM user_roles WHERE user_role_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
