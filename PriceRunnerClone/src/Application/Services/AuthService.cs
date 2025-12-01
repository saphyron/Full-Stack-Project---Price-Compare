using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(string userName, string password);
    }

    /// <summary>
    /// Authentication service.
    /// Handles username/password verification and returns a login DTO.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IDbConnection _db;

        public AuthService(IDbConnection db)
        {
            _db = db;
        }

        public async Task<LoginResponseDto?> LoginAsync(string userName, string password)
        {
            var passwordHash = ComputeSha256(password);

            const string sql = @"
            SELECT
                u.user_id          AS UserId,
                u.user_name        AS UserName,
                u.user_role_id     AS UserRoleId,
                ur.user_role_name  AS UserRoleName
            FROM users u
            LEFT JOIN user_roles ur ON ur.user_role_id = u.user_role_id
            WHERE u.user_name = @UserName
            AND u.password_hash = @PasswordHash;";

            var user = await _db.QuerySingleOrDefaultAsync<LoginResponseDto>(sql, new
            {
                UserName = userName,
                PasswordHash = passwordHash
            });

            if (user is null)
                return null;

            // Placeholder for JWT or other token logic later
            user.Token = null;

            return user;
        }

        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
