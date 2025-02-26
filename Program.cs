//using SshConnectionInfo = Renci.SshNet.ConnectionInfo;
//using HttpConnectionInfo = Microsoft.AspNetCore.Http.ConnectionInfo;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using products_api.Data;
//using products_api.Endpoints;
//using Microsoft.OpenApi.Models;
//using Renci.SshNet; // Ajouter SSH.NET pour la connexion à EC2
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
//    string privateKeyPath = @"path\to\your-key.pem";  // Remplacer par le chemin vers votre clé privée
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
//        Se connecter à EC2
//            client.Connect();
//        Console.WriteLine("Connexion à EC2 réussie");

//        Exécuter les commandes pour installer Docker et Docker Compose
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

//        Console.WriteLine("Docker installé et démarré avec succès.");

//        Se déconnecter du serveur
//            client.Disconnect();
//        Console.WriteLine("Déconnexion d'EC2");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Une erreur est survenue : {ex.Message}");
//    }
//}
//}

// Fonction pour exécuter une commande SSH
//void ExecuteCommand(SshClient client, string command)
//{
//    var cmd = client.RunCommand(command);
//Console.WriteLine($"Sortie : {cmd.Result}");
//}


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


