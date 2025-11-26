using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.Services;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Api.Endpoints
{
    public static class ProductPriceEndpoint
    {
        public static IEndpointRouteBuilder MapProductPriceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/product-prices")
                .WithTags("ProductPrices");

            // GET /api/product-prices
            group.MapGet("/", async (IPriceService svc) =>
            {
                var rows = await svc.GetAllPriceRowsAsync();
                return Results.Ok(rows);
            });

            // GET /api/product-prices/{id}
            group.MapGet("/{id:int}", async (int id, IPriceService svc) =>
            {
                var row = await svc.GetPriceRowByIdAsync(id);
                return row is null ? Results.NotFound() : Results.Ok(row);
            });

            // POST /api/product-prices
            group.MapPost("/", async (CreateProductPriceRequest req, IPriceService svc) =>
            {
                if (req.ProductId <= 0 || req.ShopId <= 0 || req.CurrentPrice < 0)
                {
                    return Results.BadRequest(new
                    {
                        errors = new[] { "ProductId og ShopId skal være > 0, og CurrentPrice skal være >= 0." }
                    });
                }

                var created = await svc.CreatePriceRowAsync(
                    req.ProductId,
                    req.ShopId,
                    req.CurrentPrice,
                    req.LastUpdatedUtc);

                return Results.Created($"/api/product-prices/{created.Id}", created);
            });

            // PUT /api/product-prices/{id}
            group.MapPut("/{id:int}", async (int id, UpdateProductPriceRequest req, IPriceService svc) =>
            {
                if (req.ProductId <= 0 || req.ShopId <= 0 || req.CurrentPrice < 0)
                {
                    return Results.BadRequest(new
                    {
                        errors = new[] { "ProductId og ShopId skal være > 0, og CurrentPrice skal være >= 0." }
                    });
                }

                var updated = await svc.UpdatePriceRowAsync(
                    id,
                    req.ProductId,
                    req.ShopId,
                    req.CurrentPrice,
                    req.LastUpdatedUtc);

                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/product-prices/{id}
            group.MapDelete("/{id:int}", async (int id, IPriceService svc) =>
            {
                var deleted = await svc.DeletePriceRowAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            return routes;
        }
    }
}
