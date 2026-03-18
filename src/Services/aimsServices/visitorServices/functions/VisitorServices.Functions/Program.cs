using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VisitorServices.Infrastructure.Data;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// EF Core for Functions
builder.Services.AddDbContext<VisitorDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("VisitorDb")
            ?? throw new InvalidOperationException("VisitorDb connection string is required.")));

builder.Build().Run();

