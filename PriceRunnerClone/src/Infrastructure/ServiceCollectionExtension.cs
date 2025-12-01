// src/Infrastructure/ServiceCollectionExtensions.cs
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PriceRunner.Infrastructure.Configurations;
using PriceRunner.Infrastructure.Data;

namespace PriceRunner.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Her kan du lave samme fallback som du havde i Program.cs
            var connectionString =
                configuration.GetConnectionString("DefaultConnection")
                ?? configuration["MYSQL_CONNECTION_STRING"]
                ?? configuration["Database:ConnectionString"]
                ?? "server=localhost;port=3306;database=price_runner;user=root;password=yourpassword;";

            services.Configure<DatabaseOptions>(opt =>
            {
                opt.ConnectionString = connectionString;
            });

            services.AddSingleton<IDbConnectionFactory, MySqlDbConnectionFactory>();
            services.AddScoped<IDbConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IDbConnectionFactory>();
                return factory.Create();
            });

            return services;
        }
    }
}
