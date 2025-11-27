// src/Infrastructure/DependencyInjection.cs
using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace PriceRunner.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1) Primært: fra ENV (sat via .env)
            // 2) Fallback: Database:ConnectionString i appsettings
            // 3) Fallback: ConnectionStrings:DefaultConnection (hvis du tilføjer det senere)
            var connectionString =
                Environment.GetEnvironmentVariable("PRICE_RUNNER_CONNECTION_STRING") ??
                configuration["Database:ConnectionString"] ??
                configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException(
                    "No MySQL connection string configured. " +
                    "Set PRICE_RUNNER_CONNECTION_STRING in .env eller Database:ConnectionString i appsettings.");

            services.AddScoped<IDbConnection>(_ =>
            {
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                return conn;
            });

            return services;
        }
    }
}
