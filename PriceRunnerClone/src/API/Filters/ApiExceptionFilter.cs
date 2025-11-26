using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MySqlConnector;

namespace PriceRunner.Api.Filters
{
    /// <summary>
    /// Global exception handler for the API layer.
    /// Konverterer unhandled exceptions til standardiserede JSON-fejl.
    /// </summary>
    public static class ApiExceptionFilter
    {
        public static IApplicationBuilder UseApiExceptionFilter(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var exception = feature?.Error;

                    var (statusCode, errorCode) = MapExceptionToStatusCode(exception);

                    context.Response.StatusCode = statusCode;
                    context.Response.ContentType = "application/json";

                    var error = new ApiErrorResponse
                    {
                        StatusCode = statusCode,
                        ErrorCode = errorCode,
                        Message = BuildMessage(exception, statusCode),
                        Path = context.Request.Path,
                        TraceId = context.TraceIdentifier,
                        // Hvis du vil vise stacktrace i dev:
                        Details = env.EnvironmentName == "Development"
                            ? exception?.ToString()
                            : null
                    };

                    var json = JsonSerializer.Serialize(
                        error,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = env.EnvironmentName == "Development"
                        });

                    await context.Response.WriteAsync(json);
                });
            });

            return app;
        }

        private static (int statusCode, string errorCode) MapExceptionToStatusCode(Exception? ex)
        {
            if (ex is null)
                return ((int)HttpStatusCode.InternalServerError, "unknown_error");

            // Validation / bad input → 400
            if (ex is ArgumentException or ArgumentNullException or FormatException)
            {
                return ((int)HttpStatusCode.BadRequest, "bad_request");
            }

            // MySQL FK-conflicts → 409
            if (ex is MySqlException mysqlEx)
            {
                // 1451 / 1452: foreign key constraint fails
                if (mysqlEx.Number == 1451 || mysqlEx.Number == 1452)
                {
                    return ((int)HttpStatusCode.Conflict, "foreign_key_conflict");
                }

                // Andre databasefejl → 503 (service unavailable)
                return ((int)HttpStatusCode.ServiceUnavailable, "database_error");
            }

            // Alt andet → 500
            return ((int)HttpStatusCode.InternalServerError, "internal_server_error");
        }

        private static string BuildMessage(Exception? ex, int statusCode)
        {
            if (ex is null)
                return "An unexpected error occurred.";

            // Kort og generisk, så vi ikke lækker for meget info i prod
            return statusCode switch
            {
                400 => ex.Message,
                409 => "The operation could not be completed due to related data (conflict).",
                503 => "A database error occurred. Please try again later.",
                _   => "An unexpected server error occurred."
            };
        }

        /// <summary>
        /// Standardiseret fejlmodel for API-responses.
        /// </summary>
        private sealed class ApiErrorResponse
        {
            public int StatusCode { get; set; }
            public string ErrorCode { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;

            public string? Path { get; set; }
            public string? TraceId { get; set; }

            // Udvidbar til flere felter (fx validation errors)
            public string? Details { get; set; }
        }
    }
}
