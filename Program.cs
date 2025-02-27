//using SshConnectionInfo = Renci.SshNet.ConnectionInfo;
//using HttpConnectionInfo = Microsoft.AspNetCore.Http.ConnectionInfo;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using products_api.Data;
//using products_api.Endpoints;
//using Microsoft.OpenApi.Models;
//using Renci.SshNet; // Ajouter SSH.NET pour la connexion � EC2
//using System;

//var builder = WebApplication.CreateBuilder(args);

//Retrieve the connection string from appsettings.json
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Add CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy.AllowAnyOrigin()  // Allow any origin
//              .AllowAnyMethod()  // Allow any HTTP method
//              .AllowAnyHeader(); // Allow any header
//    });
//});

//Add services to the container
//builder.Services.AddDbContext<ProductsDbContext>(options =>
//{
//     Configure MySQL provider (Pomelo or MySQL)
//    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
//});

//Add Swagger for API documentation (OpenAPI)
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//Enable Swagger in development mode
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//app.UseSwaggerUI();
//}

// Apply CORS policy
//app.UseCors("AllowAll");

//app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS

//Configure the HTTP request pipeline
//app.MapProductsEndpoints(); // Ensure endpoints are properly mapped

//Step 2: Connect to EC2 and install Docker and Docker Compose
//ConnectAndInstallDocker();

//app.Run();  // Run the application

//Function to handle SSH connection and installation of Docker
//void ConnectAndInstallDocker()
//{
//     Remplacer ces valeurs par vos propres informations
//    string privateKeyPath = @"path\to\your-key.pem";  // Remplacer par le chemin vers votre cl� priv�e
//string ec2PublicIp = "your-ec2-public-ip";         // Remplacer par l'IP publique de votre EC2
//string username = "ubuntu";

//var keyFile = new PrivateKeyFile(privateKeyPath);
//var authMethod = new PrivateKeyAuthenticationMethod(username, keyFile); // Correct
//var connectionInfo = new SshConnectionInfo(ec2PublicIp, 22, username, authMethod); // Correct


//var keyFile = new PrivateKeyFile(privateKeyPath);
//var connectionInfo = new SshConnectionInfo(ec2PublicIp, 22, username, authMethod);

//var authMethod = new AuthenticationMethod(username, new PrivateKeyAuthenticationMethod(username, keyFile));
//var connectionInfo = new ConnectionInfo(ec2PublicIp, 22, username, authMethod);

//using (var client = new SshClient(connectionInfo))
//{
//    try
//    {
//        Se connecter � EC2
//            client.Connect();
//        Console.WriteLine("Connexion � EC2 r�ussie");

//        Ex�cuter les commandes pour installer Docker et Docker Compose
//       var updateCommand = "sudo apt update -y";
//        var installDockerCommand = "sudo apt install -y docker.io docker-compose";
//        var startDockerCommand = "sudo systemctl start docker";
//        var enableDockerCommand = "sudo systemctl enable docker";
//        var addUserToDockerCommand = "sudo usermod -aG docker $USER";

//        ExecuteCommand(client, updateCommand);
//        ExecuteCommand(client, installDockerCommand);
//        ExecuteCommand(client, startDockerCommand);
//        ExecuteCommand(client, enableDockerCommand);
//        ExecuteCommand(client, addUserToDockerCommand);

//        Console.WriteLine("Docker install� et d�marr� avec succ�s.");

//        Se d�connecter du serveur
//            client.Disconnect();
//        Console.WriteLine("D�connexion d'EC2");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Une erreur est survenue : {ex.Message}");
//    }
//}
//}

// Fonction pour ex�cuter une commande SSH
//void ExecuteCommand(SshClient client, string command)
//{
//    var cmd = client.RunCommand(command);
//Console.WriteLine($"Sortie : {cmd.Result}");
//}


using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using products_api.Data;
using products_api.Endpoints;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Get environment variables
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "product_db";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "product-api";
var dbPass = Environment.GetEnvironmentVariable("DB_PASS") ?? "securepass";
var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION") ?? "eu-north-1"; // Change to your AWS region

var connectionString = $"Server={dbHost};Database={dbName};User ID={dbUser};Password={dbPass};";

// Configure Logging
var logFilePath = "logs/api_calls.log";
var logDirectory = Path.GetDirectoryName(logFilePath) ?? "logs";
Directory.CreateDirectory(logDirectory);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Infinite, retainedFileCountLimit: null)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure AWS CloudWatch Client
var cloudWatchClient = new AmazonCloudWatchClient(Amazon.RegionEndpoint.GetBySystemName(awsRegion));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddDbContext<ProductsDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS policy
app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Middleware to log API calls and send to CloudWatch
app.Use(async (context, next) =>
{
    await next(); // Proceed to the next middleware

    try
    {
        // Send custom metric to CloudWatch
        var metricRequest = new PutMetricDataRequest
        {
            Namespace = "ProductApi",
            MetricData = new List<MetricDatum>
            {
                new MetricDatum
                {
                    MetricName = "ApiCallCount",
                    Value = 1,
                    Unit = StandardUnit.Count,
                    TimestampUtc = DateTime.UtcNow
                }
            }
        };

        await cloudWatchClient.PutMetricDataAsync(metricRequest);

        // Log API request locally
        var logMessage = $"API called at {DateTime.UtcNow} | Path: {context.Request.Path}";
        Log.Information(logMessage);
    }
    catch (Exception ex)
    {
        Log.Error($"Failed to log API call to CloudWatch: {ex.Message}");
    }
});

// Map API endpoints
app.MapProductsEndpoints();

app.Run();





