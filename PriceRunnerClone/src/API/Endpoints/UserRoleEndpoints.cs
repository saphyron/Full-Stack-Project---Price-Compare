using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;


namespace PriceRunner.Api.Endpoints
{
    public static class UserRoleEndpoint
    {
        public static IEndpointRouteBuilder MapUserRoleEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/user-roles")
                .WithTags("UserRoles");

            // GET /api/user-roles
            group.MapGet("/", async (IUserRoleService svc) =>
            {
                var roles = await svc.GetAllAsync();
                return Results.Ok(roles);
            });

            // GET /api/user-roles/{id}
            group.MapGet("/{id:int}", async (int id, IUserRoleService svc) =>
            {
                var role = await svc.GetByIdAsync(id);
                return role is null ? Results.NotFound() : Results.Ok(role);
            });

            // POST /api/user-roles
            group.MapPost("/", async (
                CreateUserRoleRequest req,
                IUserRoleService svc,
                IUserRoleValidator validator) =>
            {
                var errors = validator.ValidateForCreate(req.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(req.Name);
                return Results.Created($"/api/user-roles/{created.Id}", created);
            });

            // PUT /api/user-roles/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateUserRoleRequest req,
                IUserRoleService svc,
                IUserRoleValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(req.Name);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, req.Name);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/user-roles/{id}
            group.MapDelete("/{id:int}", async (int id, IUserRoleService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            return routes;
        }
    }
}
