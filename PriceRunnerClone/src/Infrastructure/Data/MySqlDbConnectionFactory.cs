using System.Data;
using Microsoft.Extensions.Options;
using MySqlConnector;
using PriceRunner.Infrastructure.Configurations;

namespace PriceRunner.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }

    /// <summary>
    /// Central factory for creating open MySQL connections.
    /// Used by services that depend on IDbConnection (Dapper).
    /// </summary>
    public sealed class MySqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlDbConnectionFactory(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public IDbConnection Create()
        {
            var connection = new MySqlConnection(_connectionString);
            // Lad være med at åbne her, hvis du foretrækker "lazy open"
            connection.Open();
            return connection;
        }
    }
}
