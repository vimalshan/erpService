using ReferenceService.API;
using ReferenceService.API.Auth;
using ReferenceService.Application;
using ReferenceService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ReferenceService.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services
var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>() ?? new JwtConfiguration();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not configured in appsettings.json");

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString, builder.Configuration);
builder.Services.AddApiServices(jwtConfig);

var app = builder.Build();

// ── Auto-migrate on startup ─────────────────────────────────────
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ReferenceDbContext>();
    await dbContext.Database.MigrateAsync();
    app.Logger.LogInformation("Database migration completed successfully.");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Database migration failed. Service will continue without database.");
}

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
