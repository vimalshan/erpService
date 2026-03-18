using AuthProvider.Infrastructure;
using AuthProvider.Infrastructure.Data;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        var config = ctx.Configuration;

        // ── EF Core ──────────────────────────────────────────────────────────
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("AuthProviderDB")));

        // ── Azure Blob Storage ───────────────────────────────────────────────
        services.AddSingleton(new BlobServiceClient(
            config["AzureStorage:ConnectionString"] ?? "UseDevelopmentStorage=true"));

        // ── Application Insights ─────────────────────────────────────────────
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

await host.RunAsync();
