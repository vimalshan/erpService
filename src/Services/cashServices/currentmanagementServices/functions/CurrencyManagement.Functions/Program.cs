using Microsoft.Extensions.Hosting;
using CurrencyManagement.Application;
using CurrencyManagement.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(null!); // Configuration will be injected
    })
    .Build();

host.Run();
