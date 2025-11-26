using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;


namespace PriceRunner.Api.Endpoints
{
    public static class ShopEndpoint
    {
        public static IEndpointRouteBuilder MapShopEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/shops")
                .WithTags("Shops");

            // GET /api/shops
            group.MapGet("/", async (IShopService svc) =>
            {
                var shops = await svc.GetAllAsync();
                return Results.Ok(shops);
            });

            // GET /api/shops/{id}
            group.MapGet("/{id:int}", async (int id, IShopService svc) =>
            {
                var shop = await svc.GetByIdAsync(id);
                return shop is null ? Results.NotFound() : Results.Ok(shop);
            });

            // POST /api/shops
            group.MapPost("/", async (
                CreateShopRequest req,
                IShopService svc,
                IShopValidator validator) =>
            {
                var errors = validator.ValidateForCreate(req.FullName, req.ShopUrl, req.BrandId, req.CategoryId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(req.FullName, req.ShopUrl, req.BrandId, req.CategoryId);
                return Results.Created($"/api/shops/{created.Id}", created);
            });

            // PUT /api/shops/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateShopRequest req,
                IShopService svc,
                IShopValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(req.FullName, req.ShopUrl, req.BrandId, req.CategoryId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, req.FullName, req.ShopUrl, req.BrandId, req.CategoryId);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/shops/{id}
            group.MapDelete("/{id:int}", async (int id, IShopService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            // GET /api/shops/{id}/products
            group.MapGet("/{id:int}/products", async (int id, IShopService svc) =>
            {
                var products = await svc.GetProductsAsync(id);
                return Results.Ok(products);
            });

            // GET /api/shops/{id}/prices
            group.MapGet("/{id:int}/prices", async (int id, IShopService svc) =>
            {
                var rows = await svc.GetPricesAsync(id);
                return Results.Ok(rows);
            });

            return routes;
        }
    }
}
