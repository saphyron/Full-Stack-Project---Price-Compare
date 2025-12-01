using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;


namespace PriceRunner.Api.Endpoints
{
    public static class CategoryEndpoint
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/categories")
                .WithTags("Categories");

            // GET /api/categories
            group.MapGet("/", async (ICategoryService svc) =>
            {
                var items = await svc.GetAllAsync();
                return Results.Ok(items);
            });

            // GET /api/categories/{id}
            group.MapGet("/{id:int}", async (int id, ICategoryService svc) =>
            {
                var cat = await svc.GetByIdAsync(id);
                return cat is null ? Results.NotFound() : Results.Ok(cat);
            });

            // POST /api/categories
            group.MapPost("/", async (
                CreateCategoryRequest request,
                ICategoryService svc,
                ICategoryValidator validator) =>
            {
                var errors = validator.ValidateForCreate(request.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(request.Name);
                return Results.Created($"/api/categories/{created.Id}", created);
            });

            // PUT /api/categories/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateCategoryRequest request,
                ICategoryService svc,
                ICategoryValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(request.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, request.Name);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/categories/{id}
            group.MapDelete("/{id:int}", async (int id, ICategoryService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            return routes;
        }
    }
}
