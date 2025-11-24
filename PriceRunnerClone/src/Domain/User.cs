// src/Domain/User.cs
using System;

namespace PriceRunner.Domain
{
    public class User
    {
        private User() {}
        public User(Id id, string username, string passwordHash, UserRole role)
        {
            Id = id == Id.Empty ? 
                throw new ArgumentException("Id can not be empty.", 
                nameof(id)) : id;
            Username = string.IsNullOrWhiteSpace(username)
                ? throw new ArgumentException("Username can not be empty.", 
                nameof(username)) : username.Trim();
            PasswordHash = string.IsNullOrWhiteSpace(passwordHash)
                ? throw new ArgumentException("Username can not be empty.", 
                nameof(passwordHash)) : passwordHash.Trim();
            Role = role;

        }
        public Id Id { get; private set;} // Auto-increment, PK
        public string UserName { get; private set;} = string.Empty;
        public string PasswordHash { get; private set;} = string.Empty;
        public UserRole Role { get; private set;} // Admin / User
        public bool IsAdmin => Role == UserRole.Admin;

        public void ChangePassword(string newPasswordHash)
        {
            if(string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password is required", 
                nameof(newPasswordHash));
            PasswordHash = newPasswordHash;
        }

        public void ChangeRole (UserRole role) {Role = role;}

        public enum UserRole {User = 0, Admin = 1}
    }
}