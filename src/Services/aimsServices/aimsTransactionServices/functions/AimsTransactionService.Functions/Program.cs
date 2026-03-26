using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AimsTransactionService.Infrastructure.Data;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// EF Core for Functions
var connectionString = builder.Configuration.GetSection("ConnectionStrings")["AimsTransactionDb"]
    ?? throw new InvalidOperationException("AimsTransactionDb connection string is required.");

builder.Services.AddDbContext<AimsTransactionDbContext>(options =>
    options.UseSqlServer(connectionString));

var host = builder.Build();
await host.RunAsync();
