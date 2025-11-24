// src/Domain/User.cs
using System;

namespace PriceRunner.Domain
{
    public class User
    {
        private User() {}
        public User(string username, string passwordHash, int roleId)
        {
            UpdateUsername(username);
            ChangePassword(passwordHash);
            SetRole(roleId);
        }
        public int Id { get; private set;} // Auto-increment, PK
        public string UserName { get; private set;} = string.Empty;
        public string PasswordHash { get; private set;} = string.Empty;
        public int UserRoleId { get; private set;} // FK → user_roles.user_role_id
        public UserRole? Role { get; private set;}

        public void UpdateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.", nameof(username));

            Username = username.Trim();
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash is required.", nameof(newPasswordHash));

            PasswordHash = newPasswordHash;
        }

        public void SetRole(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentOutOfRangeException(nameof(roleId), "Role id must be positive.");

            UserRoleId = roleId;
        }
    }
}