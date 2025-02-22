using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using products_api.Data;
using products_api.Endpoints;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// R�cup�rer la cha�ne de connexion
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Ajouter les services
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Ajouter Swagger (remplace OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Activer Swagger si en mode d�veloppement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuration du pipeline HTTP
app.MapProductsEndpoints();
app.UseHttpsRedirection();

app.Run();
