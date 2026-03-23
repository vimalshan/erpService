using ReferenceService.API;
using ReferenceService.API.Auth;
using ReferenceService.Application;
using ReferenceService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services
var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>() ?? new JwtConfiguration();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=REFERENCEDB;Integrated Security=True;";

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString);
builder.Services.AddApiServices(jwtConfig);

var app = builder.Build();

// Use middleware
app.UseApiMiddleware();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Application stopped with error: {ex.Message}");
}
