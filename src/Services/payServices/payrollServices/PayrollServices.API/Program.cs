using Microsoft.EntityFrameworkCore;
using PayrollServices.API.Extensions;
using PayrollServices.Infrastructure.Data;
using PayrollServices.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

// Initialize database - create schema and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PayrollDbContext>();
    
    try
    {
        // Check if database exists first
        await context.Database.CanConnectAsync();
        
        // Try to ensure database schema is created
        // This will succeed if tables don't exist, or be skipped if they already do
        try
        {
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Database schema ensured");
        }
        catch (Exception schemaEx)
        {
            Console.WriteLine($"Note: Database schema check: {schemaEx.Message}");
            // Continue anyway - tables might already exist from manual setup
        }
        
        // Seed initial data (idempotent - will check if data exists first)
        await SeedDataBatch.SeedAsync(context);
        Console.WriteLine("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
    }
}

// Configure middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll API v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseApplicationMiddleware();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGraphQL("/graphql");
app.MapHealthChecks("/health");

await app.RunAsync();
