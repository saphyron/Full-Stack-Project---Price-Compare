using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.Services;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Api.Endpoints
{
    public static class ProductPriceHistoryEndpoint
    {
        public static IEndpointRouteBuilder MapProductPriceHistoryEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/product-price-history")
                .WithTags("ProductPriceHistory");

            // GET /api/product-price-history
            group.MapGet("/", async (IPriceService svc) =>
            {
                var rows = await svc.GetAllHistoryRowsAsync();
                return Results.Ok(rows);
            });

            // GET /api/product-price-history/{id}
            group.MapGet("/{id:int}", async (int id, IPriceService svc) =>
            {
                var row = await svc.GetHistoryRowByIdAsync(id);
                return row is null ? Results.NotFound() : Results.Ok(row);
            });

            // POST /api/product-price-history
            group.MapPost("/", async (CreateProductHistoryRequest req, IPriceService svc) =>
            {
                if (req.ProductId <= 0 || req.ShopId <= 0 || req.Price < 0)
                {
                    return Results.BadRequest(new
                    {
                        errors = new[] { "ProductId og ShopId skal være > 0, og Price skal være >= 0." }
                    });
                }

                var created = await svc.CreateHistoryRowAsync(
                    req.ProductId,
                    req.ShopId,
                    req.Price,
                    req.RecordedAtUtc);

                return Results.Created($"/api/product-price-history/{created.Id}", created);
            });

            // PUT /api/product-price-history/{id}
            group.MapPut("/{id:int}", async (int id, UpdateProductHistoryRequest req, IPriceService svc) =>
            {
                if (req.ProductId <= 0 || req.ShopId <= 0 || req.Price < 0)
                {
                    return Results.BadRequest(new
                    {
                        errors = new[] { "ProductId og ShopId skal være > 0, og Price skal være >= 0." }
                    });
                }

                var updated = await svc.UpdateHistoryRowAsync(
                    id,
                    req.ProductId,
                    req.ShopId,
                    req.Price,
                    req.RecordedAtUtc);

                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/product-price-history/{id}
            group.MapDelete("/{id:int}", async (int id, IPriceService svc) =>
            {
                var deleted = await svc.DeleteHistoryRowAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            return routes;
        }
    }
}
