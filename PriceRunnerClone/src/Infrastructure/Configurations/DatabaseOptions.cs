namespace PriceRunner.Infrastructure.Configurations
{
    /// <summary>
    /// Strongly-typed database options, bound from configuration.
    /// </summary>
    public sealed class DatabaseOptions
    {
        /// <summary>
        /// Full MySQL connection string to the price_runner database.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;
    }
}
