// src/API/Endpoints/HealthEndpoint.cs
using System;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace PriceRunner.Api.Endpoints
{
    public static class HealthEndpoint
    {
        public static IEndpointRouteBuilder MapHealthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/health")
                .WithTags("Health");

            group.MapGet("/", async (IDbConnection db) =>
            {
                try
                {
                    var value = await db.ExecuteScalarAsync<int>("SELECT 1;");
                    return Results.Ok(new
                    {
                        status = "ok",
                        database = "reachable",
                        value
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        $"Database check failed: {ex.Message}",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            });

            return routes;
        }
    }
}
