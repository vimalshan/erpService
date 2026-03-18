using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((ctx, cfg) =>
    {
        cfg.AddJsonFile("local.settings.json", optional: true, reloadOnChange: false)
           .AddEnvironmentVariables();
    })
    .ConfigureServices((ctx, services) =>
    {
        var config = ctx.Configuration;

        services.AddDbContext<PromotionFunctionsDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddHttpClient("PromotionApi", c =>
        {
            c.BaseAddress = new Uri(config["PromotionApi:BaseUrl"] ?? "http://localhost:5000");
            c.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .UseSerilog((ctx, cfg) =>
        cfg.WriteTo.Console()
           .ReadFrom.Configuration(ctx.Configuration))
    .Build();

await host.RunAsync();
