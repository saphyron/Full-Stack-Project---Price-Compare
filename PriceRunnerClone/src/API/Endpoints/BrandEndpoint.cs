using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Api.Endpoints
{
    public static class BrandEndpoint
    {
        public static IEndpointRouteBuilder MapBrandEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/brands")
                .WithTags("Brands");

            // GET /api/brands
            group.MapGet("/", async (IBrandService svc) =>
            {
                var items = await svc.GetAllAsync();
                return Results.Ok(items);
            });

            // GET /api/brands/{id}
            group.MapGet("/{id:int}", async (int id, IBrandService svc) =>
            {
                var brand = await svc.GetByIdAsync(id);
                return brand is null ? Results.NotFound() : Results.Ok(brand);
            });

            // POST /api/brands
            group.MapPost("/", async (
                CreateBrandRequest request,
                IBrandService svc,
                IBrandValidator validator) =>
            {
                var errors = validator.ValidateForCreate(request.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(request.Name);
                return Results.Created($"/api/brands/{created.Id}", created);
            });

            // PUT /api/brands/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateBrandRequest request,
                IBrandService svc,
                IBrandValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(request.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, request.Name);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/brands/{id}
            group.MapDelete("/{id:int}", async (int id, IBrandService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            // GET /api/brands/{id}/shops
            group.MapGet("/{id:int}/shops", async (int id, IBrandService svc) =>
            {
                var shops = await svc.GetShopsAsync(id);
                return Results.Ok(shops);
            });

            return routes;
        }
    }
}
