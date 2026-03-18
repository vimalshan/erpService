using CanteenUnit.Application;
using CanteenUnit.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(ctx.Configuration);
    })
    .Build();

await host.RunAsync();
