using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;


namespace PriceRunner.Api.Endpoints
{
    public static class ProductEndpoint
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/products")
                .WithTags("Products");

            // -------------------------
            // CRUD
            // -------------------------

            // GET /api/products
            group.MapGet("/", async (IProductService svc) =>
            {
                var items = await svc.GetAllAsync();
                return Results.Ok(items);
            });

            // GET /api/products/{id}
            group.MapGet("/{id:int}", async (int id, IProductService svc) =>
            {
                var product = await svc.GetByIdAsync(id);
                return product is null ? Results.NotFound() : Results.Ok(product);
            });

            // POST /api/products
            group.MapPost("/", async (
                CreateProductRequest request,
                IProductService svc,
                IProductValidator validator) =>
            {
                var errors = validator.ValidateForCreate(request.Name, request.ProductUrl, request.ShopId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(request.Name, request.ProductUrl, request.ShopId);
                return Results.Created($"/api/products/{created.Id}", created);
            });

            // PUT /api/products/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateProductRequest request,
                IProductService svc,
                IProductValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(request.Name, request.ProductUrl, request.ShopId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, request.Name, request.ProductUrl, request.ShopId);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/products/{id}
            group.MapDelete("/{id:int}", async (int id, IProductService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            // -------------------------
            // Ekstra SELECTs
            // -------------------------

            // GET /api/products/by-shop/{shopId}
            group.MapGet("/by-shop/{shopId:int}", async (int shopId, IProductService svc) =>
                Results.Ok(await svc.GetByShopAsync(shopId)));

            // GET /api/products/search?name=foo
            group.MapGet("/search", async (string? name, IProductService svc) =>
                Results.Ok(await svc.SearchAsync(name)));

            // GET /api/products/{id}/prices
            group.MapGet("/{id:int}/prices", async (int id, IPriceService priceService) =>
            {
                var prices = await priceService.GetPricesForProductAsync(id);
                return Results.Ok(prices);
            });

            // GET /api/products/{id}/cheapest
            group.MapGet("/{id:int}/cheapest", async (int id, IPriceService priceService) =>
            {
                var cheapest = await priceService.GetCheapestPriceForProductAsync(id);
                if (cheapest is null)
                    return Results.NotFound();

                return Results.Ok(cheapest);
            });

            // GET /api/products/{id}/history
            group.MapGet("/{id:int}/history", async (int id, IPriceService priceService) =>
            {
                var history = await priceService.GetHistoryForProductAsync(id);
                return Results.Ok(history);
            });

            // GET /api/products/all-prices
            group.MapGet("/all-prices", async (IProductService svc) =>
                Results.Ok(await svc.GetAllPricesAsync()));

            // GET /api/products/with-brand-category
            group.MapGet("/with-brand-category", async (IProductService svc) =>
                Results.Ok(await svc.GetWithBrandCategoryAsync()));


            return routes;
        }
    }
}
