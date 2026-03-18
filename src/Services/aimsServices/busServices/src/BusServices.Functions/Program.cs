using BusServices.Infrastructure;
using BusServices.Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((ctx, services) =>
    {
        services.AddDbContext<BusDbContext>(opts =>
            opts.UseSqlServer(ctx.Configuration.GetConnectionString("BusDb")));

        services.AddInfrastructureServices(ctx.Configuration);
    })
    .Build();

await host.RunAsync();
