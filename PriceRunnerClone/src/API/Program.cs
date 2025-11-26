// src/API/Program.cs
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using PriceRunner.Api.Endpoints;
using PriceRunner.Api.Filters;
using PriceRunner.Application.Services;
using PriceRunner.Application.Validation;



var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration["MYSQL_CONNECTION_STRING"]
    ?? "server=localhost;port=3306;database=price_runner;user=root;password=yourpassword;";

// Registrer en IDbConnection per request (scoped).
builder.Services.AddScoped<IDbConnection>(_ =>
{
    var conn = new MySqlConnection(connectionString);
    conn.Open();
    return conn;
});

// Services
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDataService, DataService>();

// Validators
builder.Services.AddScoped<IProductValidator, ProductValidator>();
builder.Services.AddScoped<IShopValidator, ShopValidator>();
builder.Services.AddScoped<IBrandValidator, BrandValidator>();
builder.Services.AddScoped<ICategoryValidator, CategoryValidator>();
builder.Services.AddScoped<IUserRoleValidator, UserRoleValidator>();
builder.Services.AddScoped<IUserValidator, UserValidator>();



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseApiExceptionFilter(app.Environment); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapProductEndpoints();
app.MapShopEndpoints();
app.MapBrandEndpoints();
app.MapCategoryEndpoints();
app.MapUserRoleEndpoints();
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapDataEndpoints();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
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
