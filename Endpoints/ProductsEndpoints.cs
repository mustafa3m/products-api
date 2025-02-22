using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using products_api.Data;

namespace products_api.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/products");

        group.MapGet("/", async (ProductsDbContext dbContext) => 
        {
            var products = await dbContext.Products.ToListAsync();
            return Results.Ok(products);
        }).WithName("GetProducts").WithOpenApi();            

        group.MapGet("/{id}", async (ProductsDbContext dbContext,  int id) => 
        {
            var product = await dbContext.Products.FindAsync(id);
            return product == null ? Results.NotFound() : Results.Ok(product);            

        }).WithName("GetProductById").WithOpenApi();

        group.MapGet("/health", () => "API Is Healthy!")
            .WithName("HealthCheck")
            .WithOpenApi(); 
    }
}
