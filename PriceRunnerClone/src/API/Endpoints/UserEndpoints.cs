using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Api.Models;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Api.Endpoints
{
    public static class UserEndpoint
    {
        public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/users")
                .WithTags("Users");

            // GET /api/users
            group.MapGet("/", async (IUserService svc) =>
            {
                var users = await svc.GetAllAsync();
                return Results.Ok(users);
            });

            // GET /api/users/{id}
            group.MapGet("/{id:int}", async (int id, IUserService svc) =>
            {
                var user = await svc.GetByIdAsync(id);
                return user is null ? Results.NotFound() : Results.Ok(user);
            });

            // GET /api/users/by-role/{roleId}
            group.MapGet("/by-role/{roleId:int}", async (int roleId, IUserService svc) =>
            {
                var users = await svc.GetByRoleAsync(roleId);
                return Results.Ok(users);
            });

            // POST /api/users
            group.MapPost("/", async (
                CreateUserRequest req,
                IUserService svc,
                IUserValidator validator) =>
            {
                var errors = validator.ValidateForCreate(req.UserName, req.Password, req.UserRoleId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var created = await svc.CreateAsync(req.UserName, req.Password, req.UserRoleId);
                return Results.Created($"/api/users/{created.Id}", created);
            });

            // PUT /api/users/{id}
            group.MapPut("/{id:int}", async (
                int id,
                UpdateUserRequest req,
                IUserService svc,
                IUserValidator validator) =>
            {
                var errors = validator.ValidateForUpdate(req.UserName, req.Password, req.UserRoleId);
                if (errors.Count > 0)
                    return Results.BadRequest(new { errors });

                var updated = await svc.UpdateAsync(id, req.UserName, req.Password, req.UserRoleId);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            // DELETE /api/users/{id}
            group.MapDelete("/{id:int}", async (int id, IUserService svc) =>
            {
                var deleted = await svc.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });

            return routes;
        }
    }
}
