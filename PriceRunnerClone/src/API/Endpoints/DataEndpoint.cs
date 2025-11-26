using System;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Api.Endpoints
{
    public static class DataEndpoint
    {
        public static IEndpointRouteBuilder MapDataEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes
                .MapGroup("/api/data")
                .WithTags("Data");

            // ----------------------------------------------------
            // 1) Fladt datasæt: alle aktuelle priser
            //    /api/data/products-flat
            // ----------------------------------------------------
            group.MapGet("/products-flat", async (IDataService svc) =>
            {
                var rows = await svc.GetProductsFlatAsync();
                return Results.Ok(rows);
            });

            // ----------------------------------------------------
            // 2) Price history datasæt (time-series)
            //    /api/data/price-history?productId=&shopId=&from=&to=
            // from/to format: yyyy-MM-dd (enkelt at kalde fra frontend)
            // ----------------------------------------------------
            group.MapGet("/price-history", async (
                int? productId,
                int? shopId,
                string? from,
                string? to,
                IDataService svc) =>
            {
                DateTime? fromDate = ParseDate(from);
                DateTime? toDate   = ParseDate(to);

                var rows = await svc.GetPriceHistoryAsync(productId, shopId, fromDate, toDate);
                return Results.Ok(rows);
            });

            // ----------------------------------------------------
            // 3) Shop stats
            //    /api/data/shop-stats
            // ----------------------------------------------------
            group.MapGet("/shop-stats", async (IDataService svc) =>
            {
                var rows = await svc.GetShopStatsAsync();
                return Results.Ok(rows);
            });

            // ----------------------------------------------------
            // 4) Brand stats
            //    /api/data/brand-stats
            // ----------------------------------------------------
            group.MapGet("/brand-stats", async (IDataService svc) =>
            {
                var rows = await svc.GetBrandStatsAsync();
                return Results.Ok(rows);
            });

            // ----------------------------------------------------
            // 5) Category stats
            //    /api/data/category-stats
            // ----------------------------------------------------
            group.MapGet("/category-stats", async (IDataService svc) =>
            {
                var rows = await svc.GetCategoryStatsAsync();
                return Results.Ok(rows);
            });

            return routes;
        }

        private static DateTime? ParseDate(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            // Simpelt: forventer yyyy-MM-dd
            if (DateTime.TryParse(value, out var dt))
            {
                return dt;
            }

            return null;
        }
    }
}
