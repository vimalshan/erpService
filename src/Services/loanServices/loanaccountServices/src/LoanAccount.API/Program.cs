using Serilog;
using LoanAccount.API.Extensions;

const string AppName = "Loan Account Service API";

try
{
    Console.WriteLine("Starting application...");
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("logs/loanaccount-.txt", rollingInterval: RollingInterval.Day);
    });

    Console.WriteLine("Building services...");
    // Add services to the container
    builder.Services.AddApiServices(builder.Configuration);

    Console.WriteLine("Building app...");
    var app = builder.Build();

    // Configure the HTTP request pipeline
    Console.WriteLine("Configuring pipeline...");
    app.ConfigureApiPipeline();

    // Seed database
    Console.WriteLine("Seeding database...");
    await app.SeedDatabaseAsync();

    Console.WriteLine($"{AppName} is starting on ports 5000 (HTTP) and 5001 (HTTPS)");
    Console.WriteLine("Application is now listening - Ready to accept requests!");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex}");
    Log.Fatal(ex, $"{AppName} terminated unexpectedly");
}
finally
{
    Console.WriteLine("Flushing logs...");
    Log.CloseAndFlush();
}
