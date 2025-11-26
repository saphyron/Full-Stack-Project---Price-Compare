// src/Domain/UserRole.cs
using System;
using System.Collections.Generic;

namespace PriceRunner.Domain
{
    public class UserRole
    {
        private readonly List<User> _users = new();

        // For ORM
        private UserRole() { }

        public UserRole(string name)
        {
            UpdateName(name);
        }

        public int Id { get; private set; }          // user_role_id
        public string Name { get; private set; } = string.Empty; // user_role_name

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name is required.", nameof(name));

            Name = name.Trim();
        }

        internal void AddUser(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            _users.Add(user);
        }
    }
}
