// src/API/Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceRunner.Api.Endpoints;
using PriceRunner.Api.Filters;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;
using PriceRunner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------
// Infrastructure: DB-setup (connection string håndteres i AddInfrastructure)
// ---------------------------------------------------------------------
builder.Services.AddInfrastructure(builder.Configuration);

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
// OpenAPI / Swagger
// ---------------------------------------------------------------------
// .NET 9 minimal OpenAPI
builder.Services.AddOpenApi();

// Swagger for dev/test
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Global exception filter
app.UseApiExceptionFilter(app.Environment);

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // .NET 9 OpenAPI endpoint
    app.MapOpenApi();
}

app.UseHttpsRedirection();

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


// Template-demo route (kan slettes hvis du vil holde projektet rent)
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
