namespace PriceRunner.Api.Models
{
    public sealed class CreateUserRoleRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public sealed class UpdateUserRoleRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
