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
    public static class AuthEndpoint
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/auth")
                .WithTags("Auth");

            // POST /api/auth/login
            group.MapPost("/login", async (LoginRequest request, IAuthService auth) =>
            {
                var user = await auth.LoginAsync(request.UserName, request.Password);
                if (user is null)
                    return Results.Unauthorized();

                return Results.Ok(user);
            });


            return routes;
        }
    }
}
