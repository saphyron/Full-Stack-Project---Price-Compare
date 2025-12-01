using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetByRoleAsync(int roleId);
        Task<UserDto> CreateAsync(string userName, string password, int? userRoleId);
        Task<bool> UpdateAsync(int id, string userName, string? password, int? userRoleId);
        Task<bool> DeleteAsync(int id);
    }

    public sealed class UserService : IUserService
    {
        private readonly IDbConnection _db;

        public UserService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            const string sql = @"
            SELECT
                u.user_id      AS Id,
                u.user_name    AS UserName,
                u.user_role_id AS UserRoleId,
                ur.user_role_name AS UserRoleName
            FROM users u
            LEFT JOIN user_roles ur ON ur.user_role_id = u.user_role_id
            ORDER BY u.user_name;";

            return await _db.QueryAsync<UserDto>(sql);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            const string sql = @"
            SELECT
                u.user_id      AS Id,
                u.user_name    AS UserName,
                u.user_role_id AS UserRoleId,
                ur.user_role_name AS UserRoleName
            FROM users u
            LEFT JOIN user_roles ur ON ur.user_role_id = u.user_role_id
            WHERE u.user_id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<UserDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<UserDto>> GetByRoleAsync(int roleId)
        {
            const string sql = @"
            SELECT
                u.user_id      AS Id,
                u.user_name    AS UserName,
                u.user_role_id AS UserRoleId,
                ur.user_role_name AS UserRoleName
            FROM users u
            LEFT JOIN user_roles ur ON ur.user_role_id = u.user_role_id
            WHERE u.user_role_id = @RoleId
            ORDER BY u.user_name;";

            return await _db.QueryAsync<UserDto>(sql, new { RoleId = roleId });
        }

        public async Task<UserDto> CreateAsync(string userName, string password, int? userRoleId)
        {
            var passwordHash = string.IsNullOrWhiteSpace(password) ? null : ComputeSha256(password);

            const string insertSql = @"
            INSERT INTO users (user_name, password_hash, user_role_id)
            VALUES (@UserName, @PasswordHash, @UserRoleId);
            SELECT LAST_INSERT_ID();";

            var newId = await _db.ExecuteScalarAsync<int>(insertSql, new
            {
                UserName = userName,
                PasswordHash = passwordHash,
                UserRoleId = userRoleId
            });

            const string getSql = @"
            SELECT
                u.user_id      AS Id,
                u.user_name    AS UserName,
                u.user_role_id AS UserRoleId,
                ur.user_role_name AS UserRoleName
            FROM users u
            LEFT JOIN user_roles ur ON ur.user_role_id = u.user_role_id
            WHERE u.user_id = @Id;";

            return await _db.QuerySingleAsync<UserDto>(getSql, new { Id = newId });
        }

        public async Task<bool> UpdateAsync(int id, string userName, string? password, int? userRoleId)
        {
            const string existsSql = "SELECT COUNT(*) FROM users WHERE user_id = @Id;";
            var count = await _db.ExecuteScalarAsync<long>(existsSql, new { Id = id });
            if (count == 0)
                return false;

            string? passwordHash = null;
            if (!string.IsNullOrWhiteSpace(password))
            {
                passwordHash = ComputeSha256(password);
            }

            const string sql = @"
            UPDATE users
            SET user_name    = @UserName,
                user_role_id = @UserRoleId,
                password_hash = COALESCE(@PasswordHash, password_hash)
            WHERE user_id = @Id;";

            await _db.ExecuteAsync(sql, new
            {
                Id = id,
                UserName = userName,
                UserRoleId = userRoleId,
                PasswordHash = passwordHash
            });

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM users WHERE user_id = @Id;";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return System.Convert.ToHexString(hash);
        }
    }
}
