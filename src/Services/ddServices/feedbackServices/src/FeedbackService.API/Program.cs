using Serilog;
using FeedbackService.Application;
using FeedbackService.Infrastructure;
using FeedbackService.API.Configuration;
using FeedbackService.API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/feedback-service-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting Feedback Service Application");

    // Add services
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApiServices(builder.Configuration);

    var app = builder.Build();

    // Apply migrations and seed database
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<FeedbackService.Infrastructure.Persistence.FeedbackDbContext>();
            await dbContext.Database.MigrateAsync();
            await FeedbackService.Infrastructure.Persistence.DatabaseSeeder.SeedAsync(dbContext);
            Log.Information("Database migrations and seeding completed successfully");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error applying migrations or seeding database. Application will continue without database persistence.");
    }

    // Configure the HTTP request pipeline
    // Enable Swagger for all environments
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback Service API v1"));

    // Health Checks UI
    app.UseHealthChecks("/health");
    app.MapHealthChecks("/health/live");
    app.MapHealthChecks("/health/ready");

    app.UseHttpsRedirection();
    app.UseRouting();

    // CORS
    app.UseCors("AllowAll");

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Exception handling middleware
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // API endpoints
    app.MapControllers();

    // GraphQL endpoint
    app.MapGraphQL("/graphql");

    // Minimal API endpoints
    MapMinimalApis(app);

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Feedback Service Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Maps minimal API endpoints
/// </summary>
static void MapMinimalApis(WebApplication app)
{
    var group = app.MapGroup("/api/feedback")
        .WithName("Feedback");

    // Health endpoint
    group.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .Produces(StatusCodes.Status200OK);
}
