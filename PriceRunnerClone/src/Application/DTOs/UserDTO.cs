namespace PriceRunner.Application.DTOs
{
    /// <summary>
    /// DTO for user roles.
    /// </summary>
    public sealed class UserRoleDto
    {
        public int Id { get; set; }                 // user_role_id
        public string Name { get; set; } = string.Empty; // user_role_name

        public int? UserCount { get; set; }         // antal users med denne rolle
    }

    /// <summary>
    /// DTO for users.
    /// </summary>
    public sealed class UserDto
    {
        public int Id { get; set; }                 // user_id
        public string UserName { get; set; } = string.Empty; // user_name

        public int? UserRoleId { get; set; }        // user_role_id
        public string? UserRoleName { get; set; }   // user_roles.user_role_name
    }

    /// <summary>
    /// DTO for login-respons (simpel).
    /// </summary>
    public sealed class LoginResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int? UserRoleId { get; set; }
        public string? UserRoleName { get; set; }

        // Placeholder til evt. JWT senere
        public string? Token { get; set; }
    }
}
