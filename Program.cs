using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using products_api.Data;
using products_api.Endpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allow any origin
              .AllowAnyMethod()  // Allow any HTTP method
              .AllowAnyHeader(); // Allow any header
    });
});

// Add services to the container
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    // Configure MySQL provider (Pomelo or MySQL)
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add Swagger for API documentation (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS policy
app.UseCors("AllowAll");

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS

// Configure the HTTP request pipeline
app.MapProductsEndpoints(); // Ensure endpoints are properly mapped

app.Run();  // Run the application




//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using products_api.Data;
//using products_api.Endpoints;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Retrieve the connection string from appsettings.json
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//// Add services to the container
//builder.Services.AddDbContext<ProductsDbContext>(options =>
//{
//    // Configure MySQL provider (Pomelo or MySQL)
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
//});

//// Add Swagger for API documentation (OpenAPI)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Enable Swagger in development mode
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Configure the HTTP request pipeline
//app.MapProductsEndpoints(); // Ensure endpoints are properly mapped
//app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS

//app.Run();  // Run the application




//using Microsoft.EntityFrameworkCore;
//using products_api.Data;
//using products_api.Endpoints;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
//builder.Services.AddDbContext<ProductsDbContext>(options =>
//{
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//// if (app.Environment.IsDevelopment())
//// {    
//app.MapOpenApi();
//// }

//app.MapProductsEndpoints();
//app.UseHttpsRedirection();
//app.Run();