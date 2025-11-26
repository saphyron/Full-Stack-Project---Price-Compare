namespace PriceRunner.Api.Models
{
    public sealed class CreateUserRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? UserRoleId { get; set; }
    }

    public sealed class UpdateUserRequest
    {
        public string UserName { get; set; } = string.Empty;

        // Hvis null eller tom → behold nuværende password.
        public string? Password { get; set; }

        public int? UserRoleId { get; set; }
    }
}
