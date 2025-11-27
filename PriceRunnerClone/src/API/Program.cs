// src/API/Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceRunner.Api.Endpoints;
using PriceRunner.Api.Filters;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;
using PriceRunner.Infrastructure;
using System.Linq;
using Microsoft.AspNetCore.Http;


// Helper til at loade .env
static void LoadEnv(string envPath)
{
    if (!File.Exists(envPath))
        return;

    foreach (var line in File.ReadAllLines(envPath))
    {
        var trimmed = line.Trim();
        if (string.IsNullOrEmpty(trimmed)) continue;
        if (trimmed.StartsWith("#")) continue; // kommentar

        var idx = trimmed.IndexOf('=', StringComparison.Ordinal);
        if (idx <= 0) continue;

        var key = trimmed[..idx].Trim();
        var value = trimmed[(idx + 1)..].Trim();

        if (!string.IsNullOrEmpty(key))
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------
// Load .env fra roden (rod/.env)
// ContentRootPath peger på PriceRunnerClone, så ".." = rod
// -------------------------------------------------------------
var envPath = Path.Combine(builder.Environment.ContentRootPath, "..", ".env");
LoadEnv(envPath);

// ---------------------------------------------------------------------
// Infrastructure: DB-setup (connection string håndteres i AddInfrastructure)
// ---------------------------------------------------------------------
PriceRunner.Infrastructure.DependencyInjection.AddInfrastructure(
    builder.Services,
    builder.Configuration
);


// ---------------------------------------------------------------------
// Application Services
// ---------------------------------------------------------------------
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDataService, DataService>();

// ---------------------------------------------------------------------
// Validators
// ---------------------------------------------------------------------
builder.Services.AddScoped<IProductValidator, ProductValidator>();
builder.Services.AddScoped<IShopValidator, ShopValidator>();
builder.Services.AddScoped<IBrandValidator, BrandValidator>();
builder.Services.AddScoped<ICategoryValidator, CategoryValidator>();
builder.Services.AddScoped<IUserRoleValidator, UserRoleValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();

// ---------------------------------------------------------------------
// CORS – Vite frontend på http://localhost:5173
// ---------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")  // Vite dev server
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // KUN hvis du senere bruger cookies/auth
    });
});


// ---------------------------------------------------------------------
// OpenAPI / Swagger – midlertidigt slået FRA pga. TypeLoadException
// ---------------------------------------------------------------------
// builder.Services.AddOpenApi();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


var app = builder.Build();

// CORS 
app.Use(async (context, next) =>
{
    var origin = context.Request.Headers["Origin"].ToString();

    if (origin == "http://localhost:5173")
    {
        context.Response.Headers["Access-Control-Allow-Origin"] = origin;
        context.Response.Headers["Vary"] = "Origin";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
    }

    // Håndter preflight direkte her
    if (HttpMethods.IsOptions(context.Request.Method))
    {
        context.Response.StatusCode = StatusCodes.Status204NoContent;
        return;
    }

    await next();
});
app.UseCors();

// Global exception filter
app.UseApiExceptionFilter(app.Environment);

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    // .NET 9 OpenAPI endpoint
    //app.MapOpenApi();
}

// Hvis du KUN har HTTP lokalt, kan du godt kommentere denne ud i dev
// ellers vil den redirecte til HTTPS-porten.
// app.UseHttpsRedirection();

// ---------------------------------------------------------------------
// Map endpoints
// ---------------------------------------------------------------------
app.MapProductEndpoints();
app.MapShopEndpoints();
app.MapBrandEndpoints();
app.MapCategoryEndpoints();
app.MapUserRoleEndpoints();
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapDataEndpoints();
app.MapProductPriceEndpoints();
app.MapProductPriceHistoryEndpoints();
app.MapHealthEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
